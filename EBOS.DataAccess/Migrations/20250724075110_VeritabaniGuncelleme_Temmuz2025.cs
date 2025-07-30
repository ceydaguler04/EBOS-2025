using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VeritabaniGuncelleme_Temmuz2025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MekanID",
                table: "Etkinlikler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Saat",
                table: "Etkinlikler",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<DateTime>(
                name: "Tarih",
                table: "Etkinlikler",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Mekanlar",
                columns: table => new
                {
                    MekanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ad = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adres = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sehir = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ilce = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Semt = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enlem = table.Column<double>(type: "double", nullable: true),
                    Boylam = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mekanlar", x => x.MekanID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_MekanID",
                table: "Etkinlikler",
                column: "MekanID");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Mekanlar_MekanID",
                table: "Etkinlikler",
                column: "MekanID",
                principalTable: "Mekanlar",
                principalColumn: "MekanID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Mekanlar_MekanID",
                table: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "Mekanlar");

            migrationBuilder.DropIndex(
                name: "IX_Etkinlikler_MekanID",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "MekanID",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "Saat",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "Tarih",
                table: "Etkinlikler");
        }
    }
}
