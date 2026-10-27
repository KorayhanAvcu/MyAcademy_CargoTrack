using CargoTrack.Business.Exceptions;
using CargoTrack.Business.Helpers;
using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.Business.Services.CargoPricing;
using CargoTrack.DataAccess.Repositories.CargoMovements;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Deliveries;
using CargoTrack.DataAccess.Repositories.DeliveryExceptions;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.DTO.DTOs.Common;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using Mapster;
using System.Security.Cryptography;

namespace CargoTrack.Business.Services.Cargos
{
    public class CargoService(ICargoRepository _cargoRepository,
                              ICargoMovementRepository _cargoMovementRepository,
                              IDeliveryRepository _deliveryRepository,
                              IDeliveryExceptionRepository _deliveryExceptionRepository,
                              IEmployeeRepository _employeeRepository,
                              IAuditLogService _auditLogService,
                              ICargoPricingService _cargoPricingService) : ICargoService
    {
        private const int MaxFailedAttempts = 3;

        // =====================================================
        // DURUM GEÇİŞ KURALLARI
        // "Bu durumdan → hangi durumlara geçilebilir?"
        // =====================================================
        private static readonly Dictionary<CargoStatus, CargoStatus[]> _allowedTransitions = new()
        {
            [CargoStatus.Created] = new[] { CargoStatus.AtOriginBranch },
            [CargoStatus.AtOriginBranch] = new[] { CargoStatus.InTransferCenter, CargoStatus.AtDestinationBranch },
            [CargoStatus.InTransferCenter] = new[] { CargoStatus.InTransferCenter, CargoStatus.AtDestinationBranch },
            [CargoStatus.AtDestinationBranch] = new[] { CargoStatus.OutForDelivery },
            [CargoStatus.OutForDelivery] = new[] { CargoStatus.Delivered, CargoStatus.DeliveryFailed },
            [CargoStatus.DeliveryFailed] = new[] { CargoStatus.OutForDelivery },          // Yeniden dağıtım
            [CargoStatus.ReturnInProgress] = new[] { CargoStatus.ReturnedToSender },
            [CargoStatus.Delivered] = Array.Empty<CargoStatus>(),
            [CargoStatus.ReturnedToSender] = Array.Empty<CargoStatus>()
        };

        // Bu durumlara genel hareket ekranından geçilemez: kod doğrulama / gerekçe zorunlu
        private static readonly CargoStatus[] _specialStatuses = { CargoStatus.Delivered, CargoStatus.DeliveryFailed };

        // =====================================================
        // OLUŞTURMA VE OPERASYON
        // =====================================================

        public async Task<string> CreateAsync(CreateCargoDto dto, Guid userId)
        {
            var cargo = dto.Adapt<Cargo>();

            // Bu alanları kullanıcı girmez, servis hesaplar
            cargo.TrackCode = await GenerateTrackCodeAsync();
            cargo.ShipmentDate = DateTime.Now;
            cargo.Desi = _cargoPricingService.CalculateDesi(dto.Width, dto.Length, dto.Height);

            var distanceType = await _cargoPricingService.GetDistanceTypeAsync(dto.OriginBranchId, dto.DestinationBranchId);
            cargo.Price = await _cargoPricingService.CalculatePriceAsync(cargo.Desi, dto.Weight, dto.DeliveryType, distanceType);
            cargo.EstimatedArrivalDate = _cargoPricingService.CalculateEstimatedArrivalDate(cargo.ShipmentDate, dto.DeliveryType, distanceType);
            cargo.CargoStatus = CargoStatus.Created;

            // İlk hareket: Cargo'nun listesine eklendiği için tek SaveChanges ile birlikte kaydedilir
            cargo.Movements = new List<CargoMovement>
            {
                new()
                {
                    OldStatus = null,
                    NewStatus = CargoStatus.Created,
                    MovementDate = DateTime.Now,
                    BranchId = dto.OriginBranchId,
                    Description = "Gönderi oluşturuldu"
                }
            };

            await _cargoRepository.CreateAsync(cargo);
            await _auditLogService.LogAsync(userId, AuditActionType.Create, nameof(Cargo), cargo.Id,
                                            null, cargo.TrackCode, "Kargo oluşturuldu");

            return cargo.TrackCode;
        }

        public async Task ChangeStatusAsync(ChangeCargoStatusDto dto, Guid userId, Guid? employeeId)
        {
            if (_specialStatuses.Contains(dto.NewStatus))
                throw new BusinessException("Teslim ve teslim edilemedi işlemleri kendi ekranlarından yapılmalıdır.");

            if (dto.BranchId.HasValue == dto.TransferCenterId.HasValue)
                throw new BusinessException("Hareket için şube veya transfer merkezinden yalnızca biri seçilmelidir.");

            var cargo = await GetCargoOrThrowAsync(dto.CargoId);
            await EnsureEmployeeCanOperateAsync(cargo, employeeId);

            var oldStatus = cargo.CargoStatus;
            EnsureTransition(oldStatus, dto.NewStatus);

            // Her dağıtıma çıkışta yeni teslimat kodu
            if (dto.NewStatus == CargoStatus.OutForDelivery)
                cargo.DeliveryCode = GenerateDeliveryCode();

            cargo.CargoStatus = dto.NewStatus;
            await _cargoRepository.UpdateAsync(cargo);

            await AddMovementAsync(cargo.Id, oldStatus, dto.NewStatus, dto.Description,
                                   dto.BranchId, dto.TransferCenterId, employeeId);
            await LogStatusChangeAsync(userId, cargo.Id, oldStatus, dto.NewStatus, dto.Description);
        }

        public async Task DeliverAsync(DeliverCargoDto dto, Guid userId, Guid employeeId)
        {
            var cargo = await GetCargoOrThrowAsync(dto.CargoId);
            await EnsureEmployeeCanOperateAsync(cargo, employeeId);
            EnsureTransition(cargo.CargoStatus, CargoStatus.Delivered);

            // Kod doğrulanmadan teslim edilemez
            if (cargo.DeliveryCode != dto.DeliveryCode?.Trim())
                throw new BusinessException("Teslimat kodu hatalı.");

            var oldStatus = cargo.CargoStatus;
            var now = DateTime.Now;

            cargo.CargoStatus = CargoStatus.Delivered;
            cargo.DeliveredDate = now;
            cargo.DeliveryCode = null;   // Kod tek kullanımlık
            await _cargoRepository.UpdateAsync(cargo);

            await _deliveryRepository.CreateAsync(new Delivery
            {
                CargoId = cargo.Id,
                EmployeeId = employeeId,
                ReceivedByName = dto.ReceivedByName.Trim(),
                DeliveredDate = now
            });

            var description = $"Teslim alan: {dto.ReceivedByName.Trim()}";
            await AddMovementAsync(cargo.Id, oldStatus, CargoStatus.Delivered, description,
                                   cargo.DestinationBranchId, null, employeeId);
            await LogStatusChangeAsync(userId, cargo.Id, oldStatus, CargoStatus.Delivered, description);
        }

        public async Task ReportFailureAsync(ReportDeliveryFailureDto dto, Guid userId, Guid employeeId)
        {
            var cargo = await GetCargoOrThrowAsync(dto.CargoId);
            await EnsureEmployeeCanOperateAsync(cargo, employeeId);
            EnsureTransition(cargo.CargoStatus, CargoStatus.DeliveryFailed);

            var oldStatus = cargo.CargoStatus;
            cargo.FailedAttemptCount++;
            cargo.DeliveryCode = null;   // Bir sonraki dağıtımda yeni kod üretilir

            var startReturn = cargo.FailedAttemptCount >= MaxFailedAttempts;
            cargo.CargoStatus = startReturn ? CargoStatus.ReturnInProgress : CargoStatus.DeliveryFailed;
            await _cargoRepository.UpdateAsync(cargo);

            await _deliveryExceptionRepository.CreateAsync(new DeliveryException
            {
                CargoId = cargo.Id,
                EmployeeId = employeeId,
                Reason = dto.Reason,
                Description = dto.Description,
                AttemptNumber = cargo.FailedAttemptCount,
                AttemptDate = DateTime.Now
            });

            var description = $"{cargo.FailedAttemptCount}. teslimat denemesi başarısız ({dto.Reason})";
            await AddMovementAsync(cargo.Id, oldStatus, CargoStatus.DeliveryFailed, description,
                                   cargo.DestinationBranchId, null, employeeId);
            await LogStatusChangeAsync(userId, cargo.Id, oldStatus, CargoStatus.DeliveryFailed, dto.Description);

            // Üçüncü başarısız denemede iade süreci otomatik başlar
            if (startReturn)
            {
                var returnDescription = $"{MaxFailedAttempts} başarısız deneme sonrası iade süreci başlatıldı";
                await AddMovementAsync(cargo.Id, CargoStatus.DeliveryFailed, CargoStatus.ReturnInProgress,
                                       returnDescription, cargo.DestinationBranchId, null, employeeId);
                await LogStatusChangeAsync(userId, cargo.Id, CargoStatus.DeliveryFailed,
                                           CargoStatus.ReturnInProgress, returnDescription);
            }
        }

        // =====================================================
        // OKUMA
        // =====================================================

        public async Task<CargoTrackingDto?> GetByTrackCodeAsync(string trackCode)
        {
            if (string.IsNullOrWhiteSpace(trackCode))
                return null;

            var cargo = await _cargoRepository.GetByTrackCodeWithMovementsAsync(trackCode.Trim().ToUpper());
            return cargo?.Adapt<CargoTrackingDto>();
        }

        public async Task<CargoDetailDto> GetDetailAsync(Guid cargoId, Guid userId, bool isAdmin, Guid? managerBranchId)
        {
            var cargo = await GetCargoOrThrowAsync(cargoId);

            // Yetki: Admin her şeyi, Manager kendi şubesini, User kendi kargosunu görür
            var isOwner = cargo.SenderId == userId || cargo.ReceiverId == userId;
            var isBranchManager = managerBranchId.HasValue &&
                                  (cargo.OriginBranchId == managerBranchId || cargo.DestinationBranchId == managerBranchId);

            if (!isAdmin && !isOwner && !isBranchManager)
                throw new UnauthorizedAccessException("Bu kargoyu görüntüleme yetkiniz yok.");

            var dto = cargo.Adapt<CargoDetailDto>();

            // Teslimat kodunu sadece alıcı görür (kurye kapıda alıcıdan ister)
            if (cargo.ReceiverId != userId)
                dto.DeliveryCode = null;

            return dto;
        }

        public Task<PagedResultDto<ResultCargoDto>> GetPagedAsync(CargoFilterDto filter)
        {
            return QueryPagedAsync(_cargoRepository.GetQueryable(), filter);
        }

        public Task<PagedResultDto<ResultCargoDto>> GetByBranchAsync(Guid branchId, CargoFilterDto filter)
        {
            var query = _cargoRepository.GetQueryable()
                .Where(c => c.OriginBranchId == branchId || c.DestinationBranchId == branchId);

            return QueryPagedAsync(query, filter);
        }

        public Task<PagedResultDto<ResultCargoDto>> GetByUserAsync(Guid userId, CargoFilterDto filter)
        {
            var query = _cargoRepository.GetQueryable();

            query = filter.Direction switch
            {
                CargoDirection.Incoming => query.Where(c => c.ReceiverId == userId),
                CargoDirection.Outgoing => query.Where(c => c.SenderId == userId),
                _ => query.Where(c => c.SenderId == userId || c.ReceiverId == userId)
            };

            return QueryPagedAsync(query, filter);
        }

        // =====================================================
        // YARDIMCI METOTLAR
        // =====================================================

        private async Task<Cargo> GetCargoOrThrowAsync(Guid cargoId)
        {
            return await _cargoRepository.GetByIdAsync(cargoId)
                   ?? throw new BusinessException("Kargo bulunamadı.");
        }

        private static void EnsureTransition(CargoStatus from, CargoStatus to)
        {
            if (!_allowedTransitions[from].Contains(to))
                throw new BusinessException($"{from} durumundan {to} durumuna geçilemez.");
        }

        // Personel yalnızca çıkış veya varış şubesi kendi şubesi olan kargoda işlem yapabilir.
        // Admin işlemlerinde employeeId null gelir ve kontrol yapılmaz.
        private async Task EnsureEmployeeCanOperateAsync(Cargo cargo, Guid? employeeId)
        {
            if (employeeId is null)
                return;

            var employee = await _employeeRepository.GetByIdAsync(employeeId.Value)
                           ?? throw new BusinessException("Personel bulunamadı.");

            if (employee.BranchId != cargo.OriginBranchId && employee.BranchId != cargo.DestinationBranchId)
                throw new UnauthorizedAccessException("Bu kargo şubenize ait değil.");
        }

        // Hareket ayrı CreateAsync ile eklenir (Update ile eklenirse EF onu INSERT değil UPDATE sanar)
        private async Task AddMovementAsync(Guid cargoId, CargoStatus? oldStatus, CargoStatus newStatus, string? description,
                                            Guid? branchId, Guid? transferCenterId, Guid? employeeId)
        {
            await _cargoMovementRepository.CreateAsync(new CargoMovement
            {
                CargoId = cargoId,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                MovementDate = DateTime.Now,
                Description = description,
                BranchId = branchId,
                TransferCenterId = transferCenterId,
                EmployeeId = employeeId
            });
        }

        private Task LogStatusChangeAsync(Guid userId, Guid cargoId, CargoStatus oldStatus, CargoStatus newStatus, string? description)
        {
            return _auditLogService.LogAsync(userId, AuditActionType.StatusChange, nameof(Cargo), cargoId,
                                             oldStatus.ToString(), newStatus.ToString(), description);
        }

        private static async Task<PagedResultDto<ResultCargoDto>> QueryPagedAsync(IQueryable<Cargo> query, CargoFilterDto filter)
        {
            return await ApplyFilter(query, filter)
                .OrderByDescending(c => c.ShipmentDate)
                .ProjectToType<ResultCargoDto>()   // DTO dönüşümü SQL'de yapılır, sadece gereken kolonlar gelir
                .ToPagedResultAsync(filter.Page, filter.PageSize);
        }

        private static IQueryable<Cargo> ApplyFilter(IQueryable<Cargo> query, CargoFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToUpper();
                query = query.Where(c => c.TrackCode.Contains(search));
            }
            if (filter.Status.HasValue)
                query = query.Where(c => c.CargoStatus == filter.Status.Value);
            if (filter.StartDate.HasValue)
                query = query.Where(c => c.ShipmentDate >= filter.StartDate.Value.Date);
            if (filter.EndDate.HasValue)
            {
                var end = filter.EndDate.Value.Date.AddDays(1);
                query = query.Where(c => c.ShipmentDate < end);
            }
            if (filter.OnlyActive)
                query = query.WhereActive();
            if (filter.OnlyDelayed)
                query = query.WhereDelayed();

            return query;
        }

        // MYC-2026-849251 formatı, benzersiz olana kadar yeniden üretir
        private async Task<string> GenerateTrackCodeAsync()
        {
            string code;
            do
            {
                code = $"MYC-{DateTime.Now.Year}-{Random.Shared.Next(100000, 1000000)}";
            }
            while (await _cargoRepository.AnyAsync(c => c.TrackCode == code));

            return code;
        }

        // Güvenlik kodu olduğu için tahmin edilemeyen rastgele sayı üreteci
        private static string GenerateDeliveryCode()
        {
            return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
        }
    }
}