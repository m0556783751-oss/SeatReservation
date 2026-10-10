using AutoMapper;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Exceptions;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Service.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public EventService(
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<EventDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var ev = await _eventRepository.GetWithDetailsAsync(id, ct)
                ?? throw new NotFoundException("Event", id);

            return _mapper.Map<EventDto>(ev);
        }

        public async Task<PagedResult<EventDto>> GetPagedAsync(
            int page, int pageSize, string? search = null, int? categoryId = null,
            CancellationToken ct = default)
        {
            var (items, totalCount) = await _eventRepository.GetPagedAsync(
                page, pageSize, search, categoryId, ct);

            return new PagedResult<EventDto>
            {
                Items = _mapper.Map<List<EventDto>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<EventDto> CreateAsync(CreateEventRequest request, CancellationToken ct = default)
        {
            _ = await _venueRepository.GetByIdAsync(request.VenueId, ct)
                ?? throw new NotFoundException("Venue", request.VenueId);

            var ev = _mapper.Map<Event>(request);

            if (request.CategoryIds.Any())
            {
                var categories = await _categoryRepository.GetByIdsAsync(request.CategoryIds, ct);
                ev.Categories = categories.ToList();
            }

            await _eventRepository.AddAsync(ev, ct);
            await _eventRepository.SaveChangesAsync(ct);

            var created = await _eventRepository.GetWithDetailsAsync(ev.Id, ct);
            return _mapper.Map<EventDto>(created);
        }

        public async Task<EventDto> UpdateAsync(int id, UpdateEventRequest request, CancellationToken ct = default)
        {
            var ev = await _eventRepository.GetWithDetailsAsync(id, ct)
                ?? throw new NotFoundException("Event", id);

            ev.Title = request.Title;
            ev.Description = request.Description;
            ev.Date = request.Date;

            ev.Categories.Clear();
            if (request.CategoryIds.Any())
            {
                var categories = await _categoryRepository.GetByIdsAsync(request.CategoryIds, ct);
                foreach (var cat in categories)
                    ev.Categories.Add(cat);
            }

            _eventRepository.Update(ev);
            await _eventRepository.SaveChangesAsync(ct);

            var updated = await _eventRepository.GetWithDetailsAsync(ev.Id, ct);
            return _mapper.Map<EventDto>(updated);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var ev = await _eventRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Event", id);

            _eventRepository.Remove(ev);
            await _eventRepository.SaveChangesAsync(ct);
        }
    }
}
