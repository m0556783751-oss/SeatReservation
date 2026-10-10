using System.ComponentModel.DataAnnotations;

namespace SeatReservation.Core.DTOs
{
    public class VenueDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int SeatCount { get; set; }
    }

    public class CreateVenueRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Address { get; set; } = string.Empty;
    }

    public class UpdateVenueRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Address { get; set; } = string.Empty;
    }
}
