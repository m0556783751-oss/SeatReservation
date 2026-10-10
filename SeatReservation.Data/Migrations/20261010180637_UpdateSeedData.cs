using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SeatReservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Music");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Shiur");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Celebration");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Community");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Description", "Title" },
                values: new object[] { new DateTime(2025, 6, 15, 19, 0, 0, 0, DateTimeKind.Utc), "Grand celebration marking the completion of the Talmud cycle", "Siyum HaShas Celebration" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Description", "Title", "VenueId" },
                values: new object[] { new DateTime(2025, 7, 5, 21, 0, 0, 0, DateTimeKind.Utc), "An evening of Hasidic music and inspiration featuring Mordechai Ben David", "Melave Malka with MBD", 1 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Description", "Title", "VenueId" },
                values: new object[] { new DateTime(2025, 6, 22, 20, 0, 0, 0, DateTimeKind.Utc), "Halacha lecture on the laws of Shabbat", "Shiur by Rav Zilberstein", 2 });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Date", "Description", "Title", "VenueId" },
                values: new object[,]
                {
                    { 4, new DateTime(2025, 10, 8, 18, 0, 0, 0, DateTimeKind.Utc), "A joyful family concert for Chol HaMoed Sukkot with Avraham Fried", "Chol HaMoed Family Concert", 1 },
                    { 5, new DateTime(2025, 8, 10, 17, 0, 0, 0, DateTimeKind.Utc), "Community celebration for the inauguration of a new Torah scroll", "Hachnasat Sefer Torah", 2 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$fZ8MfgbDwrAxBKSoqpCMZOQiTyVKN8ZjBCGnscRZe47QMIrnEzmD6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$UppqJx4DzctE3PM93J4Xrucw0z6B5O2MYwCshSuuhEmSaTizaKJMS");

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Name" },
                values: new object[] { "12 Rabbi Akiva St, Bnei Brak", "Heichal HaSimcha" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Name" },
                values: new object[] { "5 HaRav Shach St, Jerusalem", "Ulam Beit Yaakov" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Concert");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Theater");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Conference");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Sports");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Description", "Title" },
                values: new object[] { new DateTime(2025, 3, 15, 20, 0, 0, 0, DateTimeKind.Utc), "Live rock concert", "Rock Night" });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Description", "Title", "VenueId" },
                values: new object[] { new DateTime(2025, 3, 20, 19, 0, 0, 0, DateTimeKind.Utc), "Hamlet performance", "Shakespeare Evening", 2 });

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Description", "Title", "VenueId" },
                values: new object[] { new DateTime(2025, 4, 10, 9, 0, 0, 0, DateTimeKind.Utc), "Annual developer meetup", "Tech Conference 2025", 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$iF5yTmk1YCs5d7deOU7OGeBFx4kzH.kzHJBHsI9b/NGYQ0PJJvwJ2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$51vXkQFEOjgNYOFpqRtAOuIWNr2dG.YyFosdHq6WEQ26p1Ss94xm2");

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Address", "Name" },
                values: new object[] { "123 Main St", "Main Hall" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Address", "Name" },
                values: new object[] { "456 Oak Ave", "Small Theater" });
        }
    }
}
