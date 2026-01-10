using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangoParser.Migrations
{
    /// <inheritdoc />
    public partial class FixFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "avarage_rating",
                table: "ratings",
                newName: "average_rating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "average_rating",
                table: "ratings",
                newName: "avarage_rating");
        }
    }
}
