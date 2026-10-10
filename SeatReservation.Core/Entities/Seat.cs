namespace SeatReservation.Core.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public string Section { get; set; } = string.Empty;

        public int VenueId { get; set; }
        public Venue Venue { get; set; } = null!;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
