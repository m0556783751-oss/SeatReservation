using System.ComponentModel.DataAnnotations;

namespace SeatReservation.Core.DTOs
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Section { get; set; } = string.Empty;
        public int VenueId { get; set; }
        public bool IsReserved { get; set; }
    }

    public class CreateSeatRequest
    {
        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string Row { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Number { get; set; }

        [StringLength(50)]
        public string Section { get; set; } = string.Empty;
    }
}
