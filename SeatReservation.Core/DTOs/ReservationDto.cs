using SeatReservation.Core.Enums;

namespace SeatReservation.Core.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime ReservedAt { get; set; }
        public ReservationStatus Status { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public int SeatId { get; set; }
        public string SeatLabel { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }

    public class CreateReservationRequest
    {
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int EventId { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int SeatId { get; set; }
    }
}
