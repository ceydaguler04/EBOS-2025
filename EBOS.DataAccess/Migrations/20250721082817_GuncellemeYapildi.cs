using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GuncellemeYapildi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GorselYolu",
                table: "Etkinlikler",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "KullaniciID",
                table: "Etkinlikler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_KullaniciID",
                table: "Etkinlikler",
                column: "KullaniciID");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Kullanicilar_KullaniciID",
                table: "Etkinlikler",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Kullanicilar_KullaniciID",
                table: "Etkinlikler");

            migrationBuilder.DropIndex(
                name: "IX_Etkinlikler_KullaniciID",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "KullaniciID",
                table: "Etkinlikler");

            migrationBuilder.AlterColumn<string>(
                name: "GorselYolu",
                table: "Etkinlikler",
                type: "longtext",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
