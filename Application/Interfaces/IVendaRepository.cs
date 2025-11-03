using Domain.Models;

namespace Application.Interfaces
{
    public interface IVendaRepository
    {
        Task<Sale?> GetByIdAsync(int id);
        Task<IEnumerable<Sale>> GetAllAsync(int limit = 100);
        Task<IEnumerable<Sale>> GetByStoreAsync(int storeId, DateTime? from = null, DateTime? to = null);
    }
}
