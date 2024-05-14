using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Book.Migrations
{
    /// <inheritdoc />
    public partial class AppUserReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AppUser",
                table: "Reviews",
                column: "AppUser");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUser",
                table: "Reviews",
                column: "AppUser",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_AppUser",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AppUser",
                table: "Reviews");
        }
    }
}
