using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Entities;
using SeatReservation.Core.Enums;
using SeatReservation.Core.Exceptions;
using SeatReservation.Core.Interfaces;
using SeatReservation.Service.Mapping;
using SeatReservation.Service.Services;

namespace SeatReservation.Tests
{
    public class ReservationServiceTests
    {
        private readonly Mock<IReservationRepository> _reservationRepoMock;
        private readonly Mock<ISeatRepository> _seatRepoMock;
        private readonly Mock<IEventRepository> _eventRepoMock;
        private readonly IMapper _mapper;
        private readonly ReservationService _service;

        public ReservationServiceTests()
        {
            _reservationRepoMock = new Mock<IReservationRepository>();
            _seatRepoMock = new Mock<ISeatRepository>();
            _eventRepoMock = new Mock<IEventRepository>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _service = new ReservationService(
                _reservationRepoMock.Object,
                _seatRepoMock.Object,
                _eventRepoMock.Object,
                _mapper);
        }

        [Fact]
        public async Task CreateAsync_SeatAvailable_ReturnsReservation()
        {
            var request = new CreateReservationRequest { EventId = 1, SeatId = 1 };
            var userId = 1;

            _eventRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Event { Id = 1, Title = "Rock Night", VenueId = 1 });

            _seatRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Seat { Id = 1, Row = "A", Number = 1, VenueId = 1 });

            _reservationRepoMock
                .Setup(r => r.IsSeatReservedAsync(1, 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _reservationRepoMock
                .Setup(r => r.GetWithDetailsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reservation
                {
                    Id = 1,
                    EventId = 1,
                    SeatId = 1,
                    UserId = userId,
                    Status = ReservationStatus.Active,
                    Event = new Event { Id = 1, Title = "Rock Night" },
                    Seat = new Seat { Id = 1, Row = "A", Number = 1 },
                    User = new User { Id = 1, FullName = "Demo Customer" }
                });

            var result = await _service.CreateAsync(userId, request);

            Assert.NotNull(result);
            Assert.Equal(1, result.EventId);
            Assert.Equal(1, result.SeatId);
            Assert.Equal("Rock Night", result.EventTitle);
            _reservationRepoMock.Verify(r => r.AddAsync(It.IsAny<Reservation>(), It.IsAny<CancellationToken>()), Times.Once);
            _reservationRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_SeatAlreadyReserved_ThrowsConflict()
        {
            var request = new CreateReservationRequest { EventId = 1, SeatId = 1 };

            _eventRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Event { Id = 1, Title = "Rock Night", VenueId = 1 });

            _seatRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Seat { Id = 1, Row = "A", Number = 1, VenueId = 1 });

            _reservationRepoMock
                .Setup(r => r.IsSeatReservedAsync(1, 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<ConflictException>(
                () => _service.CreateAsync(1, request));

            Assert.Contains("already reserved", ex.Message);
            _reservationRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ConcurrencyConflict_ThrowsConflict()
        {
            var request = new CreateReservationRequest { EventId = 1, SeatId = 1 };

            _eventRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Event { Id = 1, Title = "Rock Night", VenueId = 1 });

            _seatRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Seat { Id = 1, Row = "A", Number = 1, VenueId = 1 });

            _reservationRepoMock
                .Setup(r => r.IsSeatReservedAsync(1, 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _reservationRepoMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateConcurrencyException(
                    "Concurrency conflict", new List<Microsoft.EntityFrameworkCore.Update.IUpdateEntry>()));

            var ex = await Assert.ThrowsAsync<ConflictException>(
                () => _service.CreateAsync(1, request));

            Assert.Contains("modified by another user", ex.Message);
        }

        [Fact]
        public async Task CancelAsync_OwnReservation_Succeeds()
        {
            var userId = 1;
            var reservation = new Reservation
            {
                Id = 1,
                UserId = userId,
                Status = ReservationStatus.Active,
                EventId = 1,
                SeatId = 1
            };

            _reservationRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            await _service.CancelAsync(1, userId);

            Assert.Equal(ReservationStatus.Cancelled, reservation.Status);
            _reservationRepoMock.Verify(r => r.Update(reservation), Times.Once);
            _reservationRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelAsync_OtherUsersReservation_ThrowsUnauthorized()
        {
            var reservation = new Reservation
            {
                Id = 1,
                UserId = 2,
                Status = ReservationStatus.Active,
                EventId = 1,
                SeatId = 1
            };

            _reservationRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reservation);

            await Assert.ThrowsAsync<UnauthorizedException>(
                () => _service.CancelAsync(1, userId: 1));
        }
    }
}
