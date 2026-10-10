using SeatReservation.Core.Entities;

namespace SeatReservation.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    }
}
