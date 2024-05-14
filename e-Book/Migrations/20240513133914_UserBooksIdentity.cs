using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_Book.Migrations
{
    /// <inheritdoc />
    public partial class UserBooksIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
               name: "PK_UserBooks",
               table: "UserBooks");
            migrationBuilder.DropColumn("Id", "UserBooks");
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserBooks",
                type: "int",
                nullable: false).Annotation("SqlServer:Identity", "1, 1");
            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBooks",
                table: "UserBooks",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
