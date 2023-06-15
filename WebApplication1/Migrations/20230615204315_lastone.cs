using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eindopdrachtcnd2.Migrations
{
    /// <inheritdoc />
    public partial class lastone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59464e38-87d9-45a4-aab7-c7da5263fff7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91571eb9-db8a-41ff-82e3-9eedc5eb104e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f4d41835-c808-43a7-a944-d24f7d1774bd");

            migrationBuilder.AddColumn<int>(
                name: "Sprint",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f40fc46-598a-4e22-b9d2-1374b62884f4", "2", "GroupAdmin", "GroupAdmin" },
                    { "504f45cf-47b0-4b62-a33c-f858bfd7db73", "3", "User", "User" },
                    { "61520c9d-2299-4f9a-a825-d38e52d8e17e", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f40fc46-598a-4e22-b9d2-1374b62884f4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "504f45cf-47b0-4b62-a33c-f858bfd7db73");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61520c9d-2299-4f9a-a825-d38e52d8e17e");

            migrationBuilder.DropColumn(
                name: "Sprint",
                table: "Cards");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "59464e38-87d9-45a4-aab7-c7da5263fff7", "3", "User", "User" },
                    { "91571eb9-db8a-41ff-82e3-9eedc5eb104e", "1", "Admin", "Admin" },
                    { "f4d41835-c808-43a7-a944-d24f7d1774bd", "2", "GroupAdmin", "GroupAdmin" }
                });
        }
    }
}
