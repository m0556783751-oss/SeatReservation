using SeatReservation.Core.Entities;

namespace SeatReservation.Core.Interfaces
{
    public interface IVenueRepository : IRepository<Venue>
    {
        Task<Venue?> GetWithSeatsAsync(int id, CancellationToken ct = default);
    }
}
