using Application.Interfaces;
using Domain.Models;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly NolaDbContext _db;

        public VendaRepository(NolaDbContext db)
        {
            _db = db;
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _db.Sales
                .AsNoTracking()
                .Include(s => s.ProductSales)
                    .ThenInclude(ps => ps.Product)
                .Include(s => s.Payments)
                .Include(s => s.DeliverySale)
                    .ThenInclude(ds => ds.DeliveryAddresses)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Sale>> GetAllAsync(int limit = 100)
        {
            return await _db.Sales
                .AsNoTracking()
                .OrderByDescending(s => s.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sale>> GetByStoreAsync(int storeId, DateTime? from = null, DateTime? to = null)
        {
            var q = _db.Sales.AsNoTracking().Where(s => s.StoreId == storeId);

            if (from.HasValue) q = q.Where(s => s.CreatedAt >= from.Value);
            if (to.HasValue) q = q.Where(s => s.CreatedAt <= to.Value);

            return await q.OrderByDescending(s => s.CreatedAt).ToListAsync();
        }
    }
}
