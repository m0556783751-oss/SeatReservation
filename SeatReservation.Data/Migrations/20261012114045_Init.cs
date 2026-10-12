using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SeatReservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Venues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VenueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Row = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Section = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VenueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    EventsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => new { x.CategoriesId, x.EventsId });
                    table.ForeignKey(
                        name: "FK_EventCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventCategories_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReservedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    SeatId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Music" },
                    { 2, "Shiur" },
                    { 3, "Celebration" },
                    { 4, "Community" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, "admin@seat.com", "Admin User", "$2a$11$Glw/jykVdQIkiSneVGCBw.Yrkb4tI6f5PRdQTTQlTCSCPag4H/.s2", 1 },
                    { 2, "customer@seat.com", "Demo Customer", "$2a$11$ht6kl3sWWML/bvrBkW1xuOIeJTf8r2GxJ.42Pte54lP7BcyWIJfFa", 0 }
                });

            migrationBuilder.InsertData(
                table: "Venues",
                columns: new[] { "Id", "Address", "Name" },
                values: new object[,]
                {
                    { 1, "12 Rabbi Akiva St, Bnei Brak", "Heichal HaSimcha" },
                    { 2, "5 HaRav Shach St, Jerusalem", "Ulam Beit Yaakov" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Date", "Description", "Title", "VenueId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 6, 15, 19, 0, 0, 0, DateTimeKind.Utc), "Grand celebration marking the completion of the Talmud cycle", "Siyum HaShas Celebration", 1 },
                    { 2, new DateTime(2025, 7, 5, 21, 0, 0, 0, DateTimeKind.Utc), "An evening of Hasidic music and inspiration featuring Mordechai Ben David", "Melaveh Malkah with MBD", 1 },
                    { 3, new DateTime(2025, 6, 22, 20, 0, 0, 0, DateTimeKind.Utc), "Halacha lecture on the laws of Shabbat", "Shiur by Rav Zilberstein", 2 },
                    { 4, new DateTime(2025, 10, 8, 18, 0, 0, 0, DateTimeKind.Utc), "A joyful family concert for Chol HaMoed Sukkot with Avraham Fried", "Chol HaMoed Family Concert", 1 },
                    { 5, new DateTime(2025, 8, 10, 17, 0, 0, 0, DateTimeKind.Utc), "Community celebration for the inauguration of a new Torah scroll", "Hachnasat Sefer Torah", 2 }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "Number", "Row", "Section", "VenueId" },
                values: new object[,]
                {
                    { 1, 1, "A", "Front", 1 },
                    { 2, 2, "A", "Front", 1 },
                    { 3, 3, "A", "Front", 1 },
                    { 4, 4, "A", "Front", 1 },
                    { 5, 5, "A", "Front", 1 },
                    { 6, 6, "A", "Front", 1 },
                    { 7, 7, "A", "Front", 1 },
                    { 8, 8, "A", "Front", 1 },
                    { 9, 1, "B", "Front", 1 },
                    { 10, 2, "B", "Front", 1 },
                    { 11, 3, "B", "Front", 1 },
                    { 12, 4, "B", "Front", 1 },
                    { 13, 5, "B", "Front", 1 },
                    { 14, 6, "B", "Front", 1 },
                    { 15, 7, "B", "Front", 1 },
                    { 16, 8, "B", "Front", 1 },
                    { 17, 1, "C", "Back", 1 },
                    { 18, 2, "C", "Back", 1 },
                    { 19, 3, "C", "Back", 1 },
                    { 20, 4, "C", "Back", 1 },
                    { 21, 5, "C", "Back", 1 },
                    { 22, 6, "C", "Back", 1 },
                    { 23, 7, "C", "Back", 1 },
                    { 24, 8, "C", "Back", 1 },
                    { 25, 1, "D", "Back", 1 },
                    { 26, 2, "D", "Back", 1 },
                    { 27, 3, "D", "Back", 1 },
                    { 28, 4, "D", "Back", 1 },
                    { 29, 5, "D", "Back", 1 },
                    { 30, 6, "D", "Back", 1 },
                    { 31, 7, "D", "Back", 1 },
                    { 32, 8, "D", "Back", 1 },
                    { 33, 1, "E", "Back", 1 },
                    { 34, 2, "E", "Back", 1 },
                    { 35, 3, "E", "Back", 1 },
                    { 36, 4, "E", "Back", 1 },
                    { 37, 5, "E", "Back", 1 },
                    { 38, 6, "E", "Back", 1 },
                    { 39, 7, "E", "Back", 1 },
                    { 40, 8, "E", "Back", 1 },
                    { 41, 1, "A", "Front", 2 },
                    { 42, 2, "A", "Front", 2 },
                    { 43, 3, "A", "Front", 2 },
                    { 44, 4, "A", "Front", 2 },
                    { 45, 5, "A", "Front", 2 },
                    { 46, 6, "A", "Front", 2 },
                    { 47, 1, "B", "Back", 2 },
                    { 48, 2, "B", "Back", 2 },
                    { 49, 3, "B", "Back", 2 },
                    { 50, 4, "B", "Back", 2 },
                    { 51, 5, "B", "Back", 2 },
                    { 52, 6, "B", "Back", 2 },
                    { 53, 1, "C", "Back", 2 },
                    { 54, 2, "C", "Back", 2 },
                    { 55, 3, "C", "Back", 2 },
                    { 56, 4, "C", "Back", 2 },
                    { 57, 5, "C", "Back", 2 },
                    { 58, 6, "C", "Back", 2 }
                });

            migrationBuilder.InsertData(
                table: "EventCategories",
                columns: new[] { "CategoriesId", "EventsId" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 4 },
                    { 2, 3 },
                    { 3, 1 },
                    { 3, 5 },
                    { 4, 1 },
                    { 4, 4 },
                    { 4, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_EventsId",
                table: "EventCategories",
                column: "EventsId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_VenueId",
                table: "Events",
                column: "VenueId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EventId",
                table: "Reservations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SeatId_EventId",
                table: "Reservations",
                columns: new[] { "SeatId", "EventId" },
                unique: true,
                filter: "\"Status\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserId",
                table: "Reservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_VenueId_Row_Number",
                table: "Seats",
                columns: new[] { "VenueId", "Row", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Venues");
        }
    }
}
