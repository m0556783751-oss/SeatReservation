using SeatReservation.Core.Entities;

namespace SeatReservation.Core.Interfaces
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<Event?> GetWithDetailsAsync(int id, CancellationToken ct = default);
        Task<(IEnumerable<Event> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize, string? search = null, int? categoryId = null,
            CancellationToken ct = default);
    }
}
