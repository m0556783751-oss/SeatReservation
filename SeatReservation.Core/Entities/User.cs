using SeatReservation.Core.Enums;

namespace SeatReservation.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Customer;

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
