using SeatReservation.Core.DTOs;

namespace SeatReservation.Core.Interfaces
{
    public interface IVenueService
    {
        Task<VenueDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<VenueDto>> GetAllAsync(CancellationToken ct = default);
        Task<VenueDto> CreateAsync(CreateVenueRequest request, CancellationToken ct = default);
        Task<VenueDto> UpdateAsync(int id, UpdateVenueRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
