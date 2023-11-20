using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLInstagram.Migrations
{
    /// <inheritdoc />
    public partial class S3Url : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "S3Url",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "S3Url",
                table: "Posts");
        }
    }
}
