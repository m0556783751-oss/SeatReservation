using SeatReservation.Core.DTOs;

namespace SeatReservation.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default);
        Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
