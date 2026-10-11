using SeatReservation.Core.Enums;

namespace SeatReservation.Core.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
        public ReservationStatus Status { get; set; } = ReservationStatus.Active;

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        public int SeatId { get; set; }
        public Seat Seat { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
