using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeatReservation.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixMelavehMalkahSpelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Melaveh Malkah with MBD");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$WFwjsS6znHccBYMo/G18OurtAt3FUPa8Mp4tp6rewherSK6BjDmqi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$ZIiAciygPcejNFKHbTrZ4ODGzE58pbNJOcP3MT5qn2RHQEIZPF/Yu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "Title",
                value: "Melave Malka with MBD");

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
    }
}
