using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MangoParser.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("genre_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mangas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    en_name = table.Column<string>(type: "text", nullable: false),
                    ru_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    last_checkout = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("manga_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tag_id_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "manga_ganre_ref",
                columns: table => new
                {
                    manga_id = table.Column<int>(type: "integer", nullable: false),
                    genre_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("manga_ganre_pk", x => new { x.manga_id, x.genre_id });
                    table.ForeignKey(
                        name: "FK_manga_ganre_ref_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_manga_ganre_ref_mangas_manga_id",
                        column: x => x.manga_id,
                        principalTable: "mangas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    avarage_rating = table.Column<decimal>(type: "numeric", nullable: false),
                    votes_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rating_id_pk", x => x.id);
                    table.ForeignKey(
                        name: "rating_manga_id_fk",
                        column: x => x.id,
                        principalTable: "mangas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "manga_tag_ref",
                columns: table => new
                {
                    manga_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("manga_tag_pk", x => new { x.manga_id, x.tag_id });
                    table.ForeignKey(
                        name: "FK_manga_tag_ref_mangas_manga_id",
                        column: x => x.manga_id,
                        principalTable: "mangas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_manga_tag_ref_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_manga_ganre_ref",
                table: "manga_ganre_ref",
                columns: new[] { "genre_id", "manga_id" });

            migrationBuilder.CreateIndex(
                name: "IX_manga_tag_ref",
                table: "manga_tag_ref",
                columns: new[] { "manga_id", "tag_id" });

            migrationBuilder.CreateIndex(
                name: "IX_manga_tag_ref_tag_id",
                table: "manga_tag_ref",
                column: "tag_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "manga_ganre_ref");

            migrationBuilder.DropTable(
                name: "manga_tag_ref");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "mangas");
        }
    }
}
