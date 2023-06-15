using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eindopdrachtcnd2.Migrations
{
    /// <inheritdoc />
    public partial class removecardtaskuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardTaskUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "575ff756-e925-4b4a-a422-919f8520de09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bc834cd-ebaa-4b34-8a7a-469ea9f71c0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca26bd43-4403-49f1-9a3d-694afe585ff1");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CardTaskUsers",
                columns: table => new
                {
                    CardTaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTaskUsers", x => new { x.CardTaskId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CardTaskUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardTaskUsers_CardTasks_CardTaskId",
                        column: x => x.CardTaskId,
                        principalTable: "CardTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "575ff756-e925-4b4a-a422-919f8520de09", "1", "Admin", "Admin" },
                    { "7bc834cd-ebaa-4b34-8a7a-469ea9f71c0c", "3", "User", "User" },
                    { "ca26bd43-4403-49f1-9a3d-694afe585ff1", "2", "GroupAdmin", "GroupAdmin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardTaskUsers_UserId",
                table: "CardTaskUsers",
                column: "UserId");
        }
    }
}
