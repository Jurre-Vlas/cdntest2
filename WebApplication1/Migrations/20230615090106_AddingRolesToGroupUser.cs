using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eindopdrachtcnd2.Migrations
{
    /// <inheritdoc />
    public partial class AddingRolesToGroupUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3b8fe78-26a4-4e38-9f20-3503ea3f88ee");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2775cd2-b3f6-43ab-bf3f-f4b2a7ff7fbf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c48a9152-6478-49b2-b910-7aa1074460fd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef9f74a9-9d36-4b96-9fa4-af413b33eb0f");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "GroupUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "260ff1d7-4eb9-42c8-b0e1-ba39d3c7e326", "3", "User", "User" },
                    { "41a48301-b769-44bb-ab0d-ca9d1f375353", "1", "Admin", "Admin" },
                    { "a7de7125-5ebd-4f1f-9235-4eb0228351b8", "2", "GroupAdmin", "GroupAdmin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "Role",
                table: "GroupUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a3b8fe78-26a4-4e38-9f20-3503ea3f88ee", "3", "User", "InterMailAmin" },
                    { "b2775cd2-b3f6-43ab-bf3f-f4b2a7ff7fbf", "2", "GroupAdmin", "MindWizeAdmin" },
                    { "c48a9152-6478-49b2-b910-7aa1074460fd", "1", "Admin", "Admin" },
                    { "ef9f74a9-9d36-4b96-9fa4-af413b33eb0f", "4", "NewUser", "NewUser" }
                });
        }
    }
}
