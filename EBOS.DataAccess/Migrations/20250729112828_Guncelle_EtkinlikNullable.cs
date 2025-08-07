using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Guncelle_EtkinlikNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Ilceler_IlceID",
                table: "Etkinlikler");

            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Mekanlar_MekanID",
                table: "Etkinlikler");

            migrationBuilder.AlterColumn<int>(
                name: "MekanID",
                table: "Etkinlikler",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IlceID",
                table: "Etkinlikler",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Ilceler_IlceID",
                table: "Etkinlikler",
                column: "IlceID",
                principalTable: "Ilceler",
                principalColumn: "IlceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Mekanlar_MekanID",
                table: "Etkinlikler",
                column: "MekanID",
                principalTable: "Mekanlar",
                principalColumn: "MekanID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Ilceler_IlceID",
                table: "Etkinlikler");

            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Mekanlar_MekanID",
                table: "Etkinlikler");

            migrationBuilder.AlterColumn<int>(
                name: "MekanID",
                table: "Etkinlikler",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IlceID",
                table: "Etkinlikler",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Ilceler_IlceID",
                table: "Etkinlikler",
                column: "IlceID",
                principalTable: "Ilceler",
                principalColumn: "IlceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Mekanlar_MekanID",
                table: "Etkinlikler",
                column: "MekanID",
                principalTable: "Mekanlar",
                principalColumn: "MekanID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
