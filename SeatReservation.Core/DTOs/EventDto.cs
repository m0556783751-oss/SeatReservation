using System.ComponentModel.DataAnnotations;

namespace SeatReservation.Core.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int VenueId { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new();
        public int AvailableSeats { get; set; }
    }

    public class CreateEventRequest
    {
        [Required]
        [StringLength(300, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Range(1, int.MaxValue)]
        public int VenueId { get; set; }

        public List<int> CategoryIds { get; set; } = new();
    }

    public class UpdateEventRequest
    {
        [Required]
        [StringLength(300, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        public List<int> CategoryIds { get; set; } = new();
    }
}
