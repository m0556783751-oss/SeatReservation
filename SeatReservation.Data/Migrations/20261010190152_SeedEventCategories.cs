using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SeatReservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedEventCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$TY1vZYt13UcUJ9YyQg7meOyUIPKRAE1L.mhd7gm8fjrB0cmQsIrGW");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$SBNH7ptzfhDs8zNeneVbSOqE3KHCTcqi4YVKgNUFnone2TefQC5wi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "EventCategories",
                keyColumns: new[] { "CategoriesId", "EventsId" },
                keyValues: new object[] { 4, 5 });

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
        }
    }
}
