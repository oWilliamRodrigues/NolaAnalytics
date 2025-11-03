using Application.Interfaces;
using Domain.Models;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly NolaDbContext _db;

        public ProductRepository(NolaDbContext db)
        {
            _db = db;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _db.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int limit = 100)
        {
            return await _db.Products
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .Take(limit)
                .ToListAsync();
        }


        public async Task<IEnumerable<Product>> GetByCategoryAsync(int? categoryId, int limit = 100)
        {
            var q = _db.Products.AsNoTracking().AsQueryable();

            if (categoryId.HasValue)
                q = q.Where(p => p.CategoryId == categoryId.Value);

            return await q.OrderBy(p => p.Name).Take(limit).ToListAsync();
        }


        public async Task<IEnumerable<(Product Product, double UnitsSold, double Revenue)>> GetTopProductsAsync(
            DateTime dateFrom,
            DateTime dateTo,
            int top = 10)
        {
            var q = from ps in _db.ProductSales.AsNoTracking()
                    join s in _db.Sales.AsNoTracking() on ps.SaleId equals s.Id
                    join p in _db.Products.AsNoTracking() on ps.ProductId equals p.Id
                    where s.CreatedAt >= dateFrom && s.CreatedAt <= dateTo && s.SaleStatusDesc == "COMPLETED"
                    group new { ps, p } by new { ps.ProductId, p } into g
                    select new
                    {
                        Product = g.Key.p,
                        Units = g.Sum(x => (double)x.ps.Quantity),
                        Revenue = g.Sum(x => (double)x.ps.TotalPrice)
                    };

            var list = await q.OrderByDescending(x => x.Units)
                              .Take(top)
                              .ToListAsync();

            return list.Select(x => (x.Product, x.Units, x.Revenue));
        }
    }
}
