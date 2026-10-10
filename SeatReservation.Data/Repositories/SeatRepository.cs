using Microsoft.EntityFrameworkCore;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Enums;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Data.Repositories
{
    public class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Seat>> GetByVenueAsync(int venueId, CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .Where(s => s.VenueId == venueId)
                .OrderBy(s => s.Row).ThenBy(s => s.Number)
                .ToListAsync(ct);

        public async Task<IEnumerable<Seat>> GetAvailableForEventAsync(int venueId, int eventId, CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .Where(s => s.VenueId == venueId &&
                    !s.Reservations.Any(r => r.EventId == eventId && r.Status == ReservationStatus.Active))
                .OrderBy(s => s.Row).ThenBy(s => s.Number)
                .ToListAsync(ct);
    }
}
