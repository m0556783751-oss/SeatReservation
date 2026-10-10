using Microsoft.EntityFrameworkCore;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Data.Repositories
{
    public class VenueRepository : Repository<Venue>, IVenueRepository
    {
        public VenueRepository(AppDbContext context) : base(context) { }

        public async Task<Venue?> GetWithSeatsAsync(int id, CancellationToken ct = default)
            => await _dbSet
                .Include(v => v.Seats)
                .FirstOrDefaultAsync(v => v.Id == id, ct);
    }
}
