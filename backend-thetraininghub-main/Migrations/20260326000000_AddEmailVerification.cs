using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace aa2_alberto_rodriguez_penalva.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agregar campo isEmailVerified a la tabla users
            migrationBuilder.AddColumn<bool>(
                name: "isEmailVerified",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            // Crear tabla emailverifications
            migrationBuilder.CreateTable(
                name: "emailverifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiresat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    isused = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emailverifications", x => x.id);
                });

            // Crear índice para búsquedas eficientes por usuario
            migrationBuilder.CreateIndex(
                name: "IX_emailverifications_userid",
                table: "emailverifications",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar tabla emailverifications
            migrationBuilder.DropTable(
                name: "emailverifications");

            // Eliminar campo isEmailVerified de la tabla users
            migrationBuilder.DropColumn(
                name: "isEmailVerified",
                table: "users");
        }
    }
}
