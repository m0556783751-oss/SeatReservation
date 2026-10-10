using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Enums;
using SeatReservation.Core.Exceptions;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.Service.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public ReservationService(
            IReservationRepository reservationRepository,
            ISeatRepository seatRepository,
            IEventRepository eventRepository,
            IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<ReservationDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var reservation = await _reservationRepository.GetWithDetailsAsync(id, ct)
                ?? throw new NotFoundException("Reservation", id);

            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<PagedResult<ReservationDto>> GetByUserAsync(
            int userId, int page, int pageSize, CancellationToken ct = default)
        {
            var (items, totalCount) = await _reservationRepository
                .GetPagedByUserAsync(userId, page, pageSize, ct);

            return new PagedResult<ReservationDto>
            {
                Items = _mapper.Map<List<ReservationDto>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ReservationDto> CreateAsync(
            int userId, CreateReservationRequest request, CancellationToken ct = default)
        {
            _ = await _eventRepository.GetByIdAsync(request.EventId, ct)
                ?? throw new NotFoundException("Event", request.EventId);

            var seat = await _seatRepository.GetByIdAsync(request.SeatId, ct)
                ?? throw new NotFoundException("Seat", request.SeatId);

            var alreadyReserved = await _reservationRepository
                .IsSeatReservedAsync(request.SeatId, request.EventId, ct);

            if (alreadyReserved)
                throw new ConflictException("This seat is already reserved for this event.");

            var reservation = new Reservation
            {
                EventId = request.EventId,
                SeatId = request.SeatId,
                UserId = userId,
                ReservedAt = DateTime.UtcNow,
                Status = ReservationStatus.Active
            };

            await _reservationRepository.AddAsync(reservation, ct);

            try
            {
                await _reservationRepository.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConflictException(
                    "The seat was modified by another user. Please refresh and try again.");
            }
            catch (DbUpdateException)
            {
                throw new ConflictException(
                    "This seat is already reserved for this event.");
            }

            var created = await _reservationRepository.GetWithDetailsAsync(reservation.Id, ct);
            return _mapper.Map<ReservationDto>(created!);
        }

        public async Task CancelAsync(int id, int userId, CancellationToken ct = default)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Reservation", id);

            if (reservation.UserId != userId)
                throw new UnauthorizedException("You can only cancel your own reservations.");

            if (reservation.Status == ReservationStatus.Cancelled)
                throw new BadRequestException("This reservation is already cancelled.");

            reservation.Status = ReservationStatus.Cancelled;
            _reservationRepository.Update(reservation);

            try
            {
                await _reservationRepository.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConflictException(
                    "The reservation was modified by another user. Please refresh and try again.");
            }
        }
    }
}
