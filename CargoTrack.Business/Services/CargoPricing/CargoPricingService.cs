using CargoTrack.Business.Exceptions;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Services.CargoPricing
{
    public class CargoPricingService(ICargoPriceRepository _cargoPriceRepository,
                                     IBranchRepository _branchRepository) : ICargoPricingService
    {
        // PDF'teki formül: En × Boy × Yükseklik / 3000
        public double CalculateDesi(double width, double length, double height)
        {
            return Math.Round(width * length * height / 3000, 2);
        }

        // İki şube aynı şehirdeyse şehir içi, değilse şehirler arası
        public async Task<DistanceType> GetDistanceTypeAsync(Guid originBranchId, Guid destinationBranchId)
        {
            var origin = await _branchRepository.GetByIdAsync(originBranchId)
                         ?? throw new BusinessException("Çıkış şubesi bulunamadı.");
            var destination = await _branchRepository.GetByIdAsync(destinationBranchId)
                              ?? throw new BusinessException("Varış şubesi bulunamadı.");

            return origin.CityId == destination.CityId ? DistanceType.SameCity : DistanceType.Intercity;
        }

        public async Task<decimal> CalculatePriceAsync(double desi, double weight, DeliveryType deliveryType, DistanceType distanceType)
        {
            // Desi ile kilodan hangisi büyükse ücret onun üzerinden hesaplanır
            var billable = Math.Max(desi, weight);

            var priceRule = await _cargoPriceRepository.GetFirstAsync(p =>
                p.DeliveryType == deliveryType &&
                p.DistanceType == distanceType &&
                p.MinDesi <= billable && billable <= p.MaxDesi);

            if (priceRule is null)
                throw new BusinessException($"{billable} desi için tanımlı bir fiyat kuralı yok.");

            return priceRule.Price;
        }

        // SLA: teslimat tipine ve mesafeye göre gün sayısı
        public DateTime CalculateEstimatedArrivalDate(DateTime shipmentDate, DeliveryType deliveryType, DistanceType distanceType)
        {
            var days = (deliveryType, distanceType) switch
            {
                (DeliveryType.Express, DistanceType.SameCity) => 1,
                (DeliveryType.Express, DistanceType.Intercity) => 2,
                (DeliveryType.Standard, DistanceType.SameCity) => 2,
                _ => 3
            };
            return shipmentDate.Date.AddDays(days);
        }
    }
}