using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analytics;

        public AnalyticsController(IAnalyticsService analytics)
        {
            _analytics = analytics;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> Overview(DateTime? from, DateTime? to, int? storeId)
        {
            var f = from ?? DateTime.UtcNow.AddDays(-30);
            var t = to ?? DateTime.UtcNow;
            var dto = await _analytics.GetOverviewAsync(f, t, storeId);
            return PartialView("_OverviewPartial", dto);
        }

        [HttpGet]
        public async Task<IActionResult> TopProducts(DateTime? from, DateTime? to, string? channel, int top = 10)
        {
            var f = from ?? DateTime.UtcNow.AddDays(-30);
            var t = to ?? DateTime.UtcNow;
            var list = await _analytics.GetTopProductsAsync(f, t, channel, top);
            return PartialView("_TopProductsPartial", list);
        }

        [HttpGet]
        public async Task<IActionResult> DeliveryByRegion(DateTime? from, DateTime? to, int minDeliveries = 10)
        {
            var f = from ?? DateTime.UtcNow.AddDays(-30);
            var t = to ?? DateTime.UtcNow;
            var list = await _analytics.GetDeliveryPerformanceByRegionAsync(f, t, minDeliveries);
            return PartialView("_DeliveryByRegionPartial", list);
        }

        [HttpGet]
        public async Task<IActionResult> ChurnedCustomers(int minPurchases = 3, int daysSince = 30)
        {
            var list = await _analytics.GetChurnedCustomersAsync(minPurchases, daysSince);
            return PartialView("_ChurnedCustomersPartial", list);
        }

        [HttpGet]
        public async Task<IActionResult> OverviewData(DateTime? from, DateTime? to, int? storeId)
        {
            var f = from ?? DateTime.UtcNow.AddDays(-30);
            var t = to ?? DateTime.UtcNow;
            var dto = await _analytics.GetOverviewAsync(f, t, storeId);
            var result = new
            {
                totalRevenue = dto.TotalRevenue,
                orders = dto.Orders,
                avgTicket = dto.AvgTicket,
                avgDeliverySeconds = dto.AvgDeliverySeconds
            };
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> TopProductsData(DateTime? from, DateTime? to, string? channel, int top = 10)
        {
            var f = from ?? DateTime.UtcNow.AddDays(-30);
            var t = to ?? DateTime.UtcNow;

            var list = await _analytics.GetTopProductsAsync(f, t, channel, top);
            var mapped = list.Select(p => new { productName = p.ProductName, units = p.UnitsSold, revenue = p.Revenue });

            return Json(mapped);
        }

        [HttpGet]
        public async Task<IActionResult> DeliveryByRegionData(DateTime? from, DateTime? to, int minDeliveries = 10)
        {
            var f = from ?? DateTime.UtcNow.AddDays(-30);
            var t = to ?? DateTime.UtcNow;
            var list = await _analytics.GetDeliveryPerformanceByRegionAsync(f, t, minDeliveries);
            var mapped = list.Select(r => new { neighborhood = r.Neighborhood, city = r.City, deliveries = r.Deliveries, avgMin = r.AvgDeliveryMinutes, p90 = r.P90Minutes });
            return Json(mapped);
        }

        [HttpGet]
        public async Task<IActionResult> ChurnedCustomersData(int minPurchases = 3, int daysSince = 30, DateTime? from = null, DateTime? to = null)
        {
            DateTime? fromUtc = from.HasValue ? DateTime.SpecifyKind(from.Value, DateTimeKind.Utc) : null;
            DateTime? toUtc = to.HasValue ? DateTime.SpecifyKind(to.Value, DateTimeKind.Utc) : null;

            var list = await _analytics.GetChurnedCustomersAsync(minPurchases, daysSince, fromUtc, toUtc);
            var mapped = list
                .OrderByDescending(c => c.LastOrder)
                .Select(c => new { customerName = c.CustomerName, totalOrders = c.TotalOrders, lastOrder = c.LastOrder });
            return Json(mapped);
        }
    }
}
