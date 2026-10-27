using CargoTrack.Entity.Entities.Enums;

namespace CargoTrack.Business.Services.CargoPricing
{
    public interface ICargoPricingService
    {
        double CalculateDesi(double width, double length, double height);
        Task<DistanceType> GetDistanceTypeAsync(Guid originBranchId, Guid destinationBranchId);
        Task<decimal> CalculatePriceAsync(double desi, double weight, DeliveryType deliveryType, DistanceType distanceType);
        DateTime CalculateEstimatedArrivalDate(DateTime shipmentDate, DeliveryType deliveryType, DistanceType distanceType);
    }
}