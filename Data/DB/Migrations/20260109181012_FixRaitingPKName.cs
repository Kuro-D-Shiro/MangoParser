using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangoParser.Migrations
{
    /// <inheritdoc />
    public partial class FixRaitingPKName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "ratings",
                newName: "manga_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "manga_id",
                table: "ratings",
                newName: "id");
        }
    }
}
