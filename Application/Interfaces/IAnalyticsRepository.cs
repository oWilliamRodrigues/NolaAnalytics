using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAnalyticsRepository
    {
        Task<OverviewDto> GetOverviewAsync(DateTime from, DateTime to, int? storeId = null);
        Task<IEnumerable<TopProductDto>> GetTopProductsAsync(DateTime from, DateTime to, string? channel = null, int top = 10);
        Task<IEnumerable<DeliveryRegionDto>> GetDeliveryPerformanceByRegionAsync(DateTime from, DateTime to, int minDeliveries = 10);
        Task<IEnumerable<ChurnCustomerDto>> GetChurnedCustomersAsync(int minPurchases = 3, int daysSince = 30, DateTime? from = null, DateTime? to = null, int? storeId = null);
    }
}
