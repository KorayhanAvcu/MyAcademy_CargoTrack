using CargoTrack.Business.Helpers;
using CargoTrack.DTO.DTOs.Cargos;
using CargoTrack.Entity.Entities;
using Mapster;

namespace CargoTrack.Business.Mappings
{
    public static class MapsterConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<CargoMovement, CargoMovementDto>.NewConfig()
                .Map(dest => dest.LocationName,
                     src => src.Branch != null ? src.Branch.Name
                          : src.TransferCenter != null ? src.TransferCenter.Name
                          : null);

            TypeAdapterConfig<Cargo, CargoTrackingDto>.NewConfig()
                .Map(dest => dest.Movements,
                     src => src.Movements.OrderByDescending(m => m.MovementDate))
                .Map(dest => dest.SenderMaskedName,
                     src => NameMasker.MaskFullName(src.Sender.FirstName, src.Sender.LastName))
                .Map(dest => dest.ReceiverMaskedName,
                     src => NameMasker.MaskFullName(src.Receiver.FirstName, src.Receiver.LastName));

            TypeAdapterConfig<Cargo, CargoDetailDto>.NewConfig()
                .Map(dest => dest.Movements,
                     src => src.Movements.OrderByDescending(m => m.MovementDate));
        }
    }
}
