using Microsoft.EntityFrameworkCore;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Enums;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Data.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        public ReservationRepository(AppDbContext context) : base(context) { }

        public async Task<Reservation?> GetWithDetailsAsync(int id, CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.Event)
                .Include(r => r.Seat)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id, ct);

        public async Task<bool> IsSeatReservedAsync(int seatId, int eventId, CancellationToken ct = default)
            => await _dbSet.AnyAsync(
                r => r.SeatId == seatId && r.EventId == eventId && r.Status == ReservationStatus.Active, ct);

        public async Task<(IEnumerable<Reservation> Items, int TotalCount)> GetPagedByUserAsync(
            int userId, int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbSet
                .Include(r => r.Event)
                .Include(r => r.Seat)
                .Include(r => r.User)
                .AsNoTracking()
                .Where(r => r.UserId == userId);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(r => r.ReservedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}
