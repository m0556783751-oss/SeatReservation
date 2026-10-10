namespace SeatReservation.Core.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public int VenueId { get; set; }
        public Venue Venue { get; set; } = null!;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
