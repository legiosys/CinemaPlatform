using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class FilmColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "Films",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Films_ImdbId",
                table: "Films",
                column: "ImdbId",
                unique: true,
                filter: "[ImdbId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Films_ImdbId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "Films");
        }
    }
}
