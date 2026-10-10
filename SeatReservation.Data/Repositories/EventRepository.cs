using Microsoft.EntityFrameworkCore;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Data.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context) { }

        public async Task<Event?> GetWithDetailsAsync(int id, CancellationToken ct = default)
            => await _dbSet
                .Include(e => e.Venue).ThenInclude(v => v.Seats)
                .Include(e => e.Categories)
                .Include(e => e.Reservations)
                .FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<(IEnumerable<Event> Items, int TotalCount)> GetPagedAsync(
            int page, int pageSize, string? search = null, int? categoryId = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .Include(e => e.Venue).ThenInclude(v => v.Seats)
                .Include(e => e.Categories)
                .Include(e => e.Reservations)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => e.Title.Contains(search) || e.Description.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(e => e.Categories.Any(c => c.Id == categoryId.Value));

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}
