using SeatReservation.Core.DTOs;

namespace SeatReservation.Core.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<ReservationDto>> GetByUserAsync(int userId, int page, int pageSize, CancellationToken ct = default);
        Task<ReservationDto> CreateAsync(int userId, CreateReservationRequest request, CancellationToken ct = default);
        Task CancelAsync(int id, int userId, CancellationToken ct = default);
    }
}
