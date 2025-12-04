using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace escuchify_api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarGeneroPrincipalAArtista : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneroPrincipal",
                table: "Artistas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneroPrincipal",
                table: "Artistas");
        }
    }
}
