using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aa2_alberto_rodriguez_penalva.Migrations
{
    /// <inheritdoc />
    public partial class AjusteUserRoom2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_purchases_itemid",
                table: "purchases",
                column: "itemid");

            migrationBuilder.CreateIndex(
                name: "IX_purchases_userid",
                table: "purchases",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_purchases_items_itemid",
                table: "purchases",
                column: "itemid",
                principalTable: "items",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_purchases_users_userid",
                table: "purchases",
                column: "userid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_purchases_items_itemid",
                table: "purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_purchases_users_userid",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "IX_purchases_itemid",
                table: "purchases");

            migrationBuilder.DropIndex(
                name: "IX_purchases_userid",
                table: "purchases");
        }
    }
}
