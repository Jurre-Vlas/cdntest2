using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eindopdrachtcnd2.Migrations
{
    /// <inheritdoc />
    public partial class changeuserIdtouserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardTaskUsers_AspNetUsers_UserId",
                table: "CardTaskUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CardUsers_AspNetUsers_UserId",
                table: "CardUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_AspNetUsers_UserId",
                table: "GroupUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "260ff1d7-4eb9-42c8-b0e1-ba39d3c7e326");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41a48301-b769-44bb-ab0d-ca9d1f375353");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7de7125-5ebd-4f1f-9235-4eb0228351b8");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GroupUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CardUsers",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_CardUsers_UserId",
                table: "CardUsers",
                newName: "IX_CardUsers_UserName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CardTaskUsers",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_CardTaskUsers_UserId",
                table: "CardTaskUsers",
                newName: "IX_CardTaskUsers_UserName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20ae2a54-babe-4265-879d-de4eed5183e3", "1", "Admin", "Admin" },
                    { "5083296c-aa27-4838-9dc3-5b80a4c073c2", "3", "User", "User" },
                    { "80e07fbc-9b74-4321-91d0-b2dc41366e10", "2", "GroupAdmin", "GroupAdmin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CardTaskUsers_AspNetUsers_UserName",
                table: "CardTaskUsers",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardUsers_AspNetUsers_UserName",
                table: "CardUsers",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_AspNetUsers_UserName",
                table: "GroupUsers",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardTaskUsers_AspNetUsers_UserName",
                table: "CardTaskUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CardUsers_AspNetUsers_UserName",
                table: "CardUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_AspNetUsers_UserName",
                table: "GroupUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20ae2a54-babe-4265-879d-de4eed5183e3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5083296c-aa27-4838-9dc3-5b80a4c073c2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "80e07fbc-9b74-4321-91d0-b2dc41366e10");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "GroupUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "CardUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CardUsers_UserName",
                table: "CardUsers",
                newName: "IX_CardUsers_UserId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "CardTaskUsers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CardTaskUsers_UserName",
                table: "CardTaskUsers",
                newName: "IX_CardTaskUsers_UserId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "260ff1d7-4eb9-42c8-b0e1-ba39d3c7e326", "3", "User", "User" },
                    { "41a48301-b769-44bb-ab0d-ca9d1f375353", "1", "Admin", "Admin" },
                    { "a7de7125-5ebd-4f1f-9235-4eb0228351b8", "2", "GroupAdmin", "GroupAdmin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CardTaskUsers_AspNetUsers_UserId",
                table: "CardTaskUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardUsers_AspNetUsers_UserId",
                table: "CardUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_AspNetUsers_UserId",
                table: "GroupUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
