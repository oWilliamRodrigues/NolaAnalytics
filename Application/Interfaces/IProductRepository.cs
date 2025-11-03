using Domain.Models;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync(int limit = 100);
        Task<IEnumerable<Product>> GetByCategoryAsync(int? categoryId, int limit = 100);
        Task<IEnumerable<(Product Product, double UnitsSold, double Revenue)>> GetTopProductsAsync(DateTime from, DateTime to, int top = 10);
    }
}
