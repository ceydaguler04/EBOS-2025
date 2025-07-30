using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MekanIlceID_Ekleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Kullanicilar_KullaniciID",
                table: "Etkinlikler");

            migrationBuilder.AddColumn<int>(
                name: "IlceID",
                table: "Mekanlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "KullaniciID",
                table: "Etkinlikler",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Kullanicilar_KullaniciID",
                table: "Etkinlikler",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Kullanicilar_KullaniciID",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "IlceID",
                table: "Mekanlar");

            migrationBuilder.AlterColumn<int>(
                name: "KullaniciID",
                table: "Etkinlikler",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Kullanicilar_KullaniciID",
                table: "Etkinlikler",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciID");
        }
    }
}
