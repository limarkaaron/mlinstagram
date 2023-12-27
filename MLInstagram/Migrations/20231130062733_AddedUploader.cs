using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLInstagram.Migrations
{
	/// <inheritdoc />
	public partial class AddedUploader : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
	name: "UploaderId",
	table: "Posts",
	type: "nvarchar(450)",
	nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Posts_UploaderId",
				table: "Posts",
				column: "UploaderId");

			migrationBuilder.AddForeignKey(
				name: "FK_Posts_AspNetUsers_UploaderId",
				table: "Posts",
				column: "UploaderId",
				principalTable: "AspNetUsers",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
