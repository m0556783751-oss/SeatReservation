using SeatReservation.Core.Entities;

namespace SeatReservation.Core.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation?> GetWithDetailsAsync(int id, CancellationToken ct = default);
        Task<bool> IsSeatReservedAsync(int seatId, int eventId, CancellationToken ct = default);
        Task<(IEnumerable<Reservation> Items, int TotalCount)> GetPagedByUserAsync(
            int userId, int page, int pageSize, CancellationToken ct = default);
    }
}
