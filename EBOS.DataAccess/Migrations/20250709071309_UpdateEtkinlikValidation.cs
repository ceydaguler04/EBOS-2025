using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEtkinlikValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_EtkinlikTurleri_EtkinlikTuruTurID",
                table: "Etkinlikler");

            migrationBuilder.DropIndex(
                name: "IX_Etkinlikler_EtkinlikTuruTurID",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "EtkinlikTuruTurID",
                table: "Etkinlikler");

            migrationBuilder.AlterColumn<string>(
                name: "GorselYolu",
                table: "Etkinlikler",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EtkinlikAdi",
                table: "Etkinlikler",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Etkinlikler",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_TurID",
                table: "Etkinlikler",
                column: "TurID");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_EtkinlikTurleri_TurID",
                table: "Etkinlikler",
                column: "TurID",
                principalTable: "EtkinlikTurleri",
                principalColumn: "TurID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_EtkinlikTurleri_TurID",
                table: "Etkinlikler");

            migrationBuilder.DropIndex(
                name: "IX_Etkinlikler_TurID",
                table: "Etkinlikler");

            migrationBuilder.AlterColumn<string>(
                name: "GorselYolu",
                table: "Etkinlikler",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EtkinlikAdi",
                table: "Etkinlikler",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Aciklama",
                table: "Etkinlikler",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "EtkinlikTuruTurID",
                table: "Etkinlikler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_EtkinlikTuruTurID",
                table: "Etkinlikler",
                column: "EtkinlikTuruTurID");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_EtkinlikTurleri_EtkinlikTuruTurID",
                table: "Etkinlikler",
                column: "EtkinlikTuruTurID",
                principalTable: "EtkinlikTurleri",
                principalColumn: "TurID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
