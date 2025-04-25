using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaryApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToDiary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Diaries");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "Diaries");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Diaries",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Diaries",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Diaries",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "Diaries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
