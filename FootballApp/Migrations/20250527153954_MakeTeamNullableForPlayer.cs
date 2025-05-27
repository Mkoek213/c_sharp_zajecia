using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeTeamNullableForPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaDomowaId",
                table: "Mecze");

            migrationBuilder.DropForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaGościId",
                table: "Mecze");

            migrationBuilder.DropForeignKey(
                name: "FK_Zawodnicy_Druzyny_DruzynaId",
                table: "Zawodnicy");

            migrationBuilder.AlterColumn<int>(
                name: "DruzynaId",
                table: "Zawodnicy",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaDomowaId",
                table: "Mecze",
                column: "DruzynaDomowaId",
                principalTable: "Druzyny",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaGościId",
                table: "Mecze",
                column: "DruzynaGościId",
                principalTable: "Druzyny",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zawodnicy_Druzyny_DruzynaId",
                table: "Zawodnicy",
                column: "DruzynaId",
                principalTable: "Druzyny",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaDomowaId",
                table: "Mecze");

            migrationBuilder.DropForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaGościId",
                table: "Mecze");

            migrationBuilder.DropForeignKey(
                name: "FK_Zawodnicy_Druzyny_DruzynaId",
                table: "Zawodnicy");

            migrationBuilder.AlterColumn<int>(
                name: "DruzynaId",
                table: "Zawodnicy",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaDomowaId",
                table: "Mecze",
                column: "DruzynaDomowaId",
                principalTable: "Druzyny",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mecze_Druzyny_DruzynaGościId",
                table: "Mecze",
                column: "DruzynaGościId",
                principalTable: "Druzyny",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zawodnicy_Druzyny_DruzynaId",
                table: "Zawodnicy",
                column: "DruzynaId",
                principalTable: "Druzyny",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
