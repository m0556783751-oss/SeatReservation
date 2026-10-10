using SeatReservation.Core.DTOs;

namespace SeatReservation.Core.Interfaces
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatDto>> GetByVenueAsync(int venueId, CancellationToken ct = default);
        Task<IEnumerable<SeatDto>> GetAvailableForEventAsync(int venueId, int eventId, CancellationToken ct = default);
        Task<SeatDto> CreateAsync(int venueId, CreateSeatRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
