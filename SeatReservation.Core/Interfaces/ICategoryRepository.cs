using SeatReservation.Core.Entities;

namespace SeatReservation.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default);
    }
}
