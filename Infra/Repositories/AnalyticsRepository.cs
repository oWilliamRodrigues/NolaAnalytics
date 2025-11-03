using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infra.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly NolaDbContext _db;

        public AnalyticsRepository(NolaDbContext db)
        {
            _db = db;
        }

        public async Task<OverviewDto> GetOverviewAsync(DateTime from, DateTime to, int? storeId = null)
        {
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            var q = _db.Sales.AsNoTracking()
                .Where(s => s.CreatedAt >= from && s.CreatedAt <= to && s.SaleStatusDesc == "COMPLETED");

            if (storeId.HasValue) q = q.Where(s => s.StoreId == storeId.Value);

            var totalRevenue = await q.SumAsync(s => (decimal?)s.TotalAmount) ?? 0m;
            var orders = await q.CountAsync();
            var avgTicket = orders > 0 ? totalRevenue / orders : 0m;
            var avgDeliverySeconds = await q.AverageAsync(s => (double?)s.DeliverySeconds) ?? 0.0;

            return new OverviewDto
            {
                TotalRevenue = totalRevenue,
                Orders = orders,
                AvgTicket = avgTicket,
                AvgDeliverySeconds = (int)Math.Round(avgDeliverySeconds)
            };
        }

        public async Task<IEnumerable<TopProductDto>> GetTopProductsAsync(DateTime from, DateTime to, string? channel = null, int top = 10)
        {
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            var q = _db.ProductSales
                       .AsNoTracking()
                       .Include(ps => ps.Product)
                       .Include(ps => ps.Sale)
                       .Where(ps => ps.Sale.CreatedAt >= from && ps.Sale.CreatedAt <= to && ps.Sale.SaleStatusDesc == "COMPLETED");

            if (!string.IsNullOrEmpty(channel))
            {
                q = q.Where(ps => ps.Sale.Channel != null && (ps.Sale.Channel.Name == channel || ps.Sale.ChannelId.ToString() == channel));
            }



            var grouped = await q
                .GroupBy(ps => new { ps.ProductId, ps.Product.Name })
                .Select(g => new TopProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    UnitsSold = g.Sum(x => (double)x.Quantity),
                    Revenue = g.Sum(x => (double)x.TotalPrice)
                })
                .OrderByDescending(x => x.UnitsSold)
                .Take(top)
                .ToListAsync();

            return grouped;
        }

        public async Task<IEnumerable<DeliveryRegionDto>> GetDeliveryPerformanceByRegionAsync(DateTime from, DateTime to, int minDeliveries = 10)
        {
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            var deliveries = await _db.Sales
                .AsNoTracking()
                .Where(s => s.CreatedAt >= from
                            && s.CreatedAt <= to
                            && s.SaleStatusDesc == "COMPLETED"
                            && s.DeliverySeconds != null)
                .Join(_db.DeliveryAddresses,
                      s => s.Id,
                      d => d.SaleId,
                      (s, d) => new { d.City, d.Neighborhood, DeliverySeconds = s.DeliverySeconds ?? 0 })
                .ToListAsync();

            var result = deliveries
                .GroupBy(x => new { x.Neighborhood, x.City })
                .Where(g => g.Count() >= minDeliveries)
                .Select(g =>
                {
                    var ordered = g.Select(x => x.DeliverySeconds).OrderBy(x => x).ToList();
                    var count = ordered.Count;
                    var avg = ordered.Average();
                    var idx90 = (int)Math.Floor(0.9 * (count - 1));
                    var p90 = ordered.Count > 0 ? ordered[Math.Max(0, idx90)] : 0;

                    return new DeliveryRegionDto
                    {
                        Neighborhood = g.Key.Neighborhood,
                        City = g.Key.City,
                        Deliveries = count,
                        AvgDeliveryMinutes = avg / 60.0,
                        P90Minutes = p90 / 60.0
                    };
                })
                .OrderByDescending(x => x.AvgDeliveryMinutes)
                .ToList();

            return result;
        }

        public async Task<IEnumerable<ChurnCustomerDto>> GetChurnedCustomersAsync( int minPurchases = 3, int daysSince = 30, DateTime? from = null, DateTime? to = null, int? storeId = null)
        {
            DateTime? fromUtc = from.HasValue ? DateTime.SpecifyKind(from.Value, DateTimeKind.Utc) : null;
            DateTime? toUtc = to.HasValue ? DateTime.SpecifyKind(to.Value, DateTimeKind.Utc) : null;

            var cutoff = DateTime.UtcNow.AddDays(-daysSince);

            var q = _db.Sales.AsNoTracking()
                .Where(s => s.SaleStatusDesc == "COMPLETED" && s.CustomerId != null);

            if (storeId.HasValue)
                q = q.Where(s => s.StoreId == storeId.Value);

            if (fromUtc.HasValue && toUtc.HasValue)
            {
                var start = fromUtc.Value.Date;
                var end = toUtc.Value.Date.AddDays(1).AddTicks(-1);
                q = q.Where(s => s.CreatedAt >= start && s.CreatedAt <= end);
            }

            var grouped = await q.GroupBy(s => s.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalOrders = g.Count(),
                    LastOrder = g.Max(x => x.CreatedAt)
                })
                .Where(x => x.TotalOrders >= minPurchases && x.LastOrder < cutoff)
                .ToListAsync();

            var result = _db.Customers
                .Where(c => grouped.Select(g => g.CustomerId).Contains(c.Id))
                .AsEnumerable()
                .Select(c =>
                {
                    var g = grouped.First(g => g.CustomerId == c.Id);
                    return new ChurnCustomerDto
                    {
                        CustomerId = c.Id,
                        CustomerName = c.CustomerName,
                        TotalOrders = g.TotalOrders,
                        LastOrder = DateTime.SpecifyKind(g.LastOrder, DateTimeKind.Utc)
                    };
                })
                .OrderByDescending(x => x.LastOrder)
                .ToList();

            return result;
        }
    }
}
