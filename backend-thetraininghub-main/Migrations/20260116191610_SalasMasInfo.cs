using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aa2_alberto_rodriguez_penalva.Migrations
{
    /// <inheritdoc />
    public partial class SalasMasInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "date",
                table: "rooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "rooms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date",
                table: "rooms");

            migrationBuilder.DropColumn(
                name: "description",
                table: "rooms");
        }
    }
}
