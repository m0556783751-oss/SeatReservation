using AutoMapper;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Exceptions;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Service.Services
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepository;
        private readonly IMapper _mapper;

        public VenueService(IVenueRepository venueRepository, IMapper mapper)
        {
            _venueRepository = venueRepository;
            _mapper = mapper;
        }

        public async Task<VenueDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var venue = await _venueRepository.GetWithSeatsAsync(id, ct)
                ?? throw new NotFoundException("Venue", id);

            return _mapper.Map<VenueDto>(venue);
        }

        public async Task<IEnumerable<VenueDto>> GetAllAsync(CancellationToken ct = default)
        {
            var venues = await _venueRepository.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<VenueDto>>(venues);
        }

        public async Task<VenueDto> CreateAsync(CreateVenueRequest request, CancellationToken ct = default)
        {
            var venue = _mapper.Map<Venue>(request);
            await _venueRepository.AddAsync(venue, ct);
            await _venueRepository.SaveChangesAsync(ct);

            return _mapper.Map<VenueDto>(venue);
        }

        public async Task<VenueDto> UpdateAsync(int id, UpdateVenueRequest request, CancellationToken ct = default)
        {
            var venue = await _venueRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Venue", id);

            _mapper.Map(request, venue);
            _venueRepository.Update(venue);
            await _venueRepository.SaveChangesAsync(ct);

            return _mapper.Map<VenueDto>(venue);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var venue = await _venueRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Venue", id);

            _venueRepository.Remove(venue);
            await _venueRepository.SaveChangesAsync(ct);
        }
    }
}
