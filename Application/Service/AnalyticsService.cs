using Application.DTOs;
using Application.Interfaces;
using Domain.Models;

namespace Application.Service
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _repo;

        public AnalyticsService(IAnalyticsRepository repo)
        {
            _repo = repo;
        }

        public Task<OverviewDto> GetOverviewAsync(DateTime from, DateTime to, int? storeId = null)
            => _repo.GetOverviewAsync(from, to, storeId);

        public Task<IEnumerable<TopProductDto>> GetTopProductsAsync(DateTime from, DateTime to, string? channel = null, int top = 10)
            => _repo.GetTopProductsAsync(from, to, channel, top);

        public Task<IEnumerable<DeliveryRegionDto>> GetDeliveryPerformanceByRegionAsync(DateTime from, DateTime to, int minDeliveries = 10)
            => _repo.GetDeliveryPerformanceByRegionAsync(from, to, minDeliveries);

        public Task<IEnumerable<ChurnCustomerDto>> GetChurnedCustomersAsync(int minPurchases = 3, int daysSince = 30, DateTime? from = null, DateTime? to = null, int? storeId = null)
            => _repo.GetChurnedCustomersAsync(minPurchases, daysSince, from, to, storeId);
    }
}
