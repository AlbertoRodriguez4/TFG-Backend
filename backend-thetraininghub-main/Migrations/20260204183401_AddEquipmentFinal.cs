using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aa2_alberto_rodriguez_penalva.Migrations
{
    /// <inheritdoc />
    public partial class AddEquipmentFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "equippedEnduranceId",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "equippedStrengthId",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_equippedEnduranceId",
                table: "users",
                column: "equippedEnduranceId");

            migrationBuilder.CreateIndex(
                name: "IX_users_equippedStrengthId",
                table: "users",
                column: "equippedStrengthId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_items_equippedEnduranceId",
                table: "users",
                column: "equippedEnduranceId",
                principalTable: "items",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_items_equippedStrengthId",
                table: "users",
                column: "equippedStrengthId",
                principalTable: "items",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_items_equippedEnduranceId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_items_equippedStrengthId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_equippedEnduranceId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_equippedStrengthId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "equippedEnduranceId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "equippedStrengthId",
                table: "users");
        }
    }
}
