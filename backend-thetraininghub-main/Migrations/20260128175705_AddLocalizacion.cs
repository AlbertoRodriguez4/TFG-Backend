using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aa2_alberto_rodriguez_penalva.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "localization",
                table: "rooms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "localization",
                table: "rooms");
        }
    }
}
