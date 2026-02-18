using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aa2_alberto_rodriguez_penalva.Migrations
{
    /// <inheritdoc />
    public partial class AddUserImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatarUrl",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatarUrl",
                table: "users");
        }
    }
}
