using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aa2_alberto_rodriguez_penalva.Migrations
{
    /// <inheritdoc />
    public partial class TasksTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isCompleted",
                table: "tasks",
                newName: "iscompleted");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "tasks",
                newName: "createdat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "iscompleted",
                table: "tasks",
                newName: "isCompleted");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "tasks",
                newName: "createdAt");
        }
    }
}
