using AutoMapper;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Exceptions;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Service.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly IMapper _mapper;

        public SeatService(ISeatRepository seatRepository, IVenueRepository venueRepository, IMapper mapper)
        {
            _seatRepository = seatRepository;
            _venueRepository = venueRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeatDto>> GetByVenueAsync(int venueId, CancellationToken ct = default)
        {
            _ = await _venueRepository.GetByIdAsync(venueId, ct)
                ?? throw new NotFoundException("Venue", venueId);

            var seats = await _seatRepository.GetByVenueAsync(venueId, ct);
            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }

        public async Task<IEnumerable<SeatDto>> GetAvailableForEventAsync(int venueId, int eventId, CancellationToken ct = default)
        {
            var seats = await _seatRepository.GetAvailableForEventAsync(venueId, eventId, ct);
            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }

        public async Task<SeatDto> CreateAsync(int venueId, CreateSeatRequest request, CancellationToken ct = default)
        {
            _ = await _venueRepository.GetByIdAsync(venueId, ct)
                ?? throw new NotFoundException("Venue", venueId);

            var seat = _mapper.Map<Seat>(request);
            seat.VenueId = venueId;

            await _seatRepository.AddAsync(seat, ct);
            await _seatRepository.SaveChangesAsync(ct);

            return _mapper.Map<SeatDto>(seat);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var seat = await _seatRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Seat", id);

            _seatRepository.Remove(seat);
            await _seatRepository.SaveChangesAsync(ct);
        }
    }
}
