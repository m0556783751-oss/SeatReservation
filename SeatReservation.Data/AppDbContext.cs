using Microsoft.EntityFrameworkCore;
using SeatReservation.Core.Entities;

namespace SeatReservation.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.FullName).HasMaxLength(100).IsRequired();
                e.Property(u => u.Email).HasMaxLength(200).IsRequired();
                e.Property(u => u.PasswordHash).IsRequired();
            });

            // Venue
            modelBuilder.Entity<Venue>(e =>
            {
                e.Property(v => v.Name).HasMaxLength(200).IsRequired();
                e.Property(v => v.Address).HasMaxLength(500).IsRequired();
            });

            // Seat
            modelBuilder.Entity<Seat>(e =>
            {
                e.Property(s => s.Row).HasMaxLength(10).IsRequired();
                e.Property(s => s.Section).HasMaxLength(50);
                e.HasOne(s => s.Venue)
                    .WithMany(v => v.Seats)
                    .HasForeignKey(s => s.VenueId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(s => new { s.VenueId, s.Row, s.Number }).IsUnique();
            });

            // Event
            modelBuilder.Entity<Event>(e =>
            {
                e.Property(ev => ev.Title).HasMaxLength(300).IsRequired();
                e.Property(ev => ev.Description).HasMaxLength(2000);
                e.HasOne(ev => ev.Venue)
                    .WithMany(v => v.Events)
                    .HasForeignKey(ev => ev.VenueId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasMany(ev => ev.Categories)
                    .WithMany(c => c.Events)
                    .UsingEntity(j =>
                    {
                        j.ToTable("EventCategories");
                        j.HasData(
                            new { CategoriesId = 3, EventsId = 1 },
                            new { CategoriesId = 4, EventsId = 1 },
                            new { CategoriesId = 1, EventsId = 2 },
                            new { CategoriesId = 2, EventsId = 3 },
                            new { CategoriesId = 1, EventsId = 4 },
                            new { CategoriesId = 4, EventsId = 4 },
                            new { CategoriesId = 3, EventsId = 5 },
                            new { CategoriesId = 4, EventsId = 5 }
                        );
                    });
            });

            // Reservation — the concurrency-protected entity
            modelBuilder.Entity<Reservation>(e =>
            {
                e.UseXminAsConcurrencyToken();
                e.HasOne(r => r.Event)
                    .WithMany(ev => ev.Reservations)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.Seat)
                    .WithMany(s => s.Reservations)
                    .HasForeignKey(r => r.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(r => r.User)
                    .WithMany(u => u.Reservations)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasIndex(r => new { r.SeatId, r.EventId })
                    .HasFilter("\"Status\" = 0")
                    .IsUnique();
            });

            // Category
            modelBuilder.Entity<Category>(e =>
            {
                e.Property(c => c.Name).HasMaxLength(100).IsRequired();
                e.HasIndex(c => c.Name).IsUnique();
            });

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "Admin User", Email = "admin@seat.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), Role = Core.Enums.Role.Admin },
                new User { Id = 2, FullName = "Demo Customer", Email = "customer@seat.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer123!"), Role = Core.Enums.Role.Customer }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Music" },
                new Category { Id = 2, Name = "Shiur" },
                new Category { Id = 3, Name = "Celebration" },
                new Category { Id = 4, Name = "Community" }
            );

            modelBuilder.Entity<Venue>().HasData(
                new Venue { Id = 1, Name = "Heichal HaSimcha", Address = "12 Rabbi Akiva St, Bnei Brak" },
                new Venue { Id = 2, Name = "Ulam Beit Yaakov", Address = "5 HaRav Shach St, Jerusalem" }
            );

            var seats = new List<Seat>();
            int seatId = 1;
            foreach (int venueId in new[] { 1, 2 })
            {
                int rows = venueId == 1 ? 5 : 3;
                int seatsPerRow = venueId == 1 ? 8 : 6;
                for (int r = 0; r < rows; r++)
                {
                    for (int s = 1; s <= seatsPerRow; s++)
                    {
                        seats.Add(new Seat
                        {
                            Id = seatId++,
                            Row = ((char)('A' + r)).ToString(),
                            Number = s,
                            Section = r < rows / 2 ? "Front" : "Back",
                            VenueId = venueId
                        });
                    }
                }
            }
            modelBuilder.Entity<Seat>().HasData(seats);

            modelBuilder.Entity<Event>().HasData(
                new Event { Id = 1, Title = "Siyum HaShas Celebration", Description = "Grand celebration marking the completion of the Talmud cycle", Date = new DateTime(2025, 6, 15, 19, 0, 0, DateTimeKind.Utc), VenueId = 1 },
                new Event { Id = 2, Title = "Melaveh Malkah with MBD", Description = "An evening of Hasidic music and inspiration featuring Mordechai Ben David", Date = new DateTime(2025, 7, 5, 21, 0, 0, DateTimeKind.Utc), VenueId = 1 },
                new Event { Id = 3, Title = "Shiur by Rav Zilberstein", Description = "Halacha lecture on the laws of Shabbat", Date = new DateTime(2025, 6, 22, 20, 0, 0, DateTimeKind.Utc), VenueId = 2 },
                new Event { Id = 4, Title = "Chol HaMoed Family Concert", Description = "A joyful family concert for Chol HaMoed Sukkot with Avraham Fried", Date = new DateTime(2025, 10, 8, 18, 0, 0, DateTimeKind.Utc), VenueId = 1 },
                new Event { Id = 5, Title = "Hachnasat Sefer Torah", Description = "Community celebration for the inauguration of a new Torah scroll", Date = new DateTime(2025, 8, 10, 17, 0, 0, DateTimeKind.Utc), VenueId = 2 }
            );

        }
    }
}
