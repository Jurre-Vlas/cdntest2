using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eindopdrachtcnd2.Migrations
{
    /// <inheritdoc />
    public partial class setusernameidkeyback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_UserName",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e487c9d-c81b-4fe0-b5be-fc9c43e24191");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c887745-57f2-484a-ad14-842bbe31afd1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "62d19f6e-0a6b-4f51-a407-2fca17525d04");

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

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "575ff756-e925-4b4a-a422-919f8520de09", "1", "Admin", "Admin" },
                    { "7bc834cd-ebaa-4b34-8a7a-469ea9f71c0c", "3", "User", "User" },
                    { "ca26bd43-4403-49f1-9a3d-694afe585ff1", "2", "GroupAdmin", "GroupAdmin" }
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: "575ff756-e925-4b4a-a422-919f8520de09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bc834cd-ebaa-4b34-8a7a-469ea9f71c0c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ca26bd43-4403-49f1-9a3d-694afe585ff1");

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "UserName",
                keyValue: null,
                column: "UserName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e487c9d-c81b-4fe0-b5be-fc9c43e24191", "2", "GroupAdmin", "GroupAdmin" },
                    { "2c887745-57f2-484a-ad14-842bbe31afd1", "1", "Admin", "Admin" },
                    { "62d19f6e-0a6b-4f51-a407-2fca17525d04", "3", "User", "User" }
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
    }
}
