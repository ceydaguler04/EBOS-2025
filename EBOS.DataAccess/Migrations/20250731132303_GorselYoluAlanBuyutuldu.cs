using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GorselYoluAlanBuyutuldu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_Koltuklar_KoltukID",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_Kullanicilar_KullaniciID",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_Seanslar_SeansID",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Koltuklar_Salonlar_SalonID",
                table: "Koltuklar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Koltuklar",
                table: "Koltuklar");

            migrationBuilder.RenameTable(
                name: "Koltuklar",
                newName: "koltuklar");

            migrationBuilder.RenameIndex(
                name: "IX_Koltuklar_SalonID",
                table: "koltuklar",
                newName: "IX_koltuklar_SalonID");

            migrationBuilder.AddColumn<string>(
                name: "Kategori",
                table: "Etkinlikler",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "KoltukID1",
                table: "Biletler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KullaniciID1",
                table: "Biletler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeansID1",
                table: "Biletler",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_koltuklar",
                table: "koltuklar",
                column: "KoltukID");

            migrationBuilder.CreateTable(
                name: "Yorumlar",
                columns: table => new
                {
                    YorumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EtkinlikAdi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    Icerik = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yorumlar", x => x.YorumID);
                    table.ForeignKey(
                        name: "FK_Yorumlar_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_KoltukID1",
                table: "Biletler",
                column: "KoltukID1");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_KullaniciID1",
                table: "Biletler",
                column: "KullaniciID1");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_SeansID1",
                table: "Biletler",
                column: "SeansID1");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_KullaniciID",
                table: "Yorumlar",
                column: "KullaniciID");

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_Kullanicilar_KullaniciID",
                table: "Biletler",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_Kullanicilar_KullaniciID1",
                table: "Biletler",
                column: "KullaniciID1",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciID");

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_Seanslar_SeansID",
                table: "Biletler",
                column: "SeansID",
                principalTable: "Seanslar",
                principalColumn: "SeansID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_Seanslar_SeansID1",
                table: "Biletler",
                column: "SeansID1",
                principalTable: "Seanslar",
                principalColumn: "SeansID");

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_koltuklar_KoltukID",
                table: "Biletler",
                column: "KoltukID",
                principalTable: "koltuklar",
                principalColumn: "KoltukID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_koltuklar_KoltukID1",
                table: "Biletler",
                column: "KoltukID1",
                principalTable: "koltuklar",
                principalColumn: "KoltukID");

            migrationBuilder.AddForeignKey(
                name: "FK_koltuklar_Salonlar_SalonID",
                table: "koltuklar",
                column: "SalonID",
                principalTable: "Salonlar",
                principalColumn: "SalonID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_Kullanicilar_KullaniciID",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_Kullanicilar_KullaniciID1",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_Seanslar_SeansID",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_Seanslar_SeansID1",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_koltuklar_KoltukID",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_Biletler_koltuklar_KoltukID1",
                table: "Biletler");

            migrationBuilder.DropForeignKey(
                name: "FK_koltuklar_Salonlar_SalonID",
                table: "koltuklar");

            migrationBuilder.DropTable(
                name: "Yorumlar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_koltuklar",
                table: "koltuklar");

            migrationBuilder.DropIndex(
                name: "IX_Biletler_KoltukID1",
                table: "Biletler");

            migrationBuilder.DropIndex(
                name: "IX_Biletler_KullaniciID1",
                table: "Biletler");

            migrationBuilder.DropIndex(
                name: "IX_Biletler_SeansID1",
                table: "Biletler");

            migrationBuilder.DropColumn(
                name: "Kategori",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "KoltukID1",
                table: "Biletler");

            migrationBuilder.DropColumn(
                name: "KullaniciID1",
                table: "Biletler");

            migrationBuilder.DropColumn(
                name: "SeansID1",
                table: "Biletler");

            migrationBuilder.RenameTable(
                name: "koltuklar",
                newName: "Koltuklar");

            migrationBuilder.RenameIndex(
                name: "IX_koltuklar_SalonID",
                table: "Koltuklar",
                newName: "IX_Koltuklar_SalonID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Koltuklar",
                table: "Koltuklar",
                column: "KoltukID");

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_Koltuklar_KoltukID",
                table: "Biletler",
                column: "KoltukID",
                principalTable: "Koltuklar",
                principalColumn: "KoltukID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_Kullanicilar_KullaniciID",
                table: "Biletler",
                column: "KullaniciID",
                principalTable: "Kullanicilar",
                principalColumn: "KullaniciID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Biletler_Seanslar_SeansID",
                table: "Biletler",
                column: "SeansID",
                principalTable: "Seanslar",
                principalColumn: "SeansID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Koltuklar_Salonlar_SalonID",
                table: "Koltuklar",
                column: "SalonID",
                principalTable: "Salonlar",
                principalColumn: "SalonID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
