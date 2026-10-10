using Microsoft.EntityFrameworkCore;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email, ct);
    }
}
