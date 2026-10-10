using AutoMapper;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Exceptions;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default)
        {
            var categories = await _categoryRepository.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
        {
            var existing = await _categoryRepository.FindAsync(c => c.Name == request.Name, ct);
            if (existing.Any())
                throw new ConflictException($"Category '{request.Name}' already exists.");

            var category = _mapper.Map<Category>(request);
            await _categoryRepository.AddAsync(category, ct);
            await _categoryRepository.SaveChangesAsync(ct);

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Category", id);

            _categoryRepository.Remove(category);
            await _categoryRepository.SaveChangesAsync(ct);
        }
    }
}
