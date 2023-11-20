using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLInstagram.Migrations
{
    /// <inheritdoc />
    public partial class RemovedLengthAndWidthAndDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Duration",
                table: "Posts",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
