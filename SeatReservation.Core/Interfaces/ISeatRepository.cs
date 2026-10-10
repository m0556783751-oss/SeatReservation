using SeatReservation.Core.Entities;

namespace SeatReservation.Core.Interfaces
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<IEnumerable<Seat>> GetByVenueAsync(int venueId, CancellationToken ct = default);
        Task<IEnumerable<Seat>> GetAvailableForEventAsync(int venueId, int eventId, CancellationToken ct = default);
    }
}
