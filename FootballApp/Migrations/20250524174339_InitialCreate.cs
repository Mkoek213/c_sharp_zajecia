using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Druzyny",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", nullable: false),
                    Miasto = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Druzyny", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mecze",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DruzynaDomowaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DruzynaGościId = table.Column<int>(type: "INTEGER", nullable: false),
                    WynikDomowy = table.Column<int>(type: "INTEGER", nullable: false),
                    WynikGości = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mecze", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mecze_Druzyny_DruzynaDomowaId",
                        column: x => x.DruzynaDomowaId,
                        principalTable: "Druzyny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mecze_Druzyny_DruzynaGościId",
                        column: x => x.DruzynaGościId,
                        principalTable: "Druzyny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Zawodnicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Imie = table.Column<string>(type: "TEXT", nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", nullable: false),
                    Pozycja = table.Column<string>(type: "TEXT", nullable: false),
                    DruzynaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zawodnicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zawodnicy_Druzyny_DruzynaId",
                        column: x => x.DruzynaId,
                        principalTable: "Druzyny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatystykiZawodnikow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ZawodnikId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mecze = table.Column<int>(type: "INTEGER", nullable: false),
                    Gole = table.Column<int>(type: "INTEGER", nullable: false),
                    Asysty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatystykiZawodnikow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatystykiZawodnikow_Zawodnicy_ZawodnikId",
                        column: x => x.ZawodnikId,
                        principalTable: "Zawodnicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mecze_DruzynaDomowaId",
                table: "Mecze",
                column: "DruzynaDomowaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mecze_DruzynaGościId",
                table: "Mecze",
                column: "DruzynaGościId");

            migrationBuilder.CreateIndex(
                name: "IX_StatystykiZawodnikow_ZawodnikId",
                table: "StatystykiZawodnikow",
                column: "ZawodnikId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zawodnicy_DruzynaId",
                table: "Zawodnicy",
                column: "DruzynaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mecze");

            migrationBuilder.DropTable(
                name: "StatystykiZawodnikow");

            migrationBuilder.DropTable(
                name: "Zawodnicy");

            migrationBuilder.DropTable(
                name: "Druzyny");
        }
    }
}
