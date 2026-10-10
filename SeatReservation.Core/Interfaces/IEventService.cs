using SeatReservation.Core.DTOs;

namespace SeatReservation.Core.Interfaces
{
    public interface IEventService
    {
        Task<EventDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PagedResult<EventDto>> GetPagedAsync(int page, int pageSize, string? search = null, int? categoryId = null, CancellationToken ct = default);
        Task<EventDto> CreateAsync(CreateEventRequest request, CancellationToken ct = default);
        Task<EventDto> UpdateAsync(int id, UpdateEventRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
