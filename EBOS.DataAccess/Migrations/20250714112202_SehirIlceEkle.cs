using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SehirIlceEkle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeansAdi",
                table: "Seanslar",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "KoltukNo",
                table: "Koltuklar",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "IlceID",
                table: "Etkinlikler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sehirler",
                columns: table => new
                {
                    SehirID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SehirAdi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sehirler", x => x.SehirID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ilceler",
                columns: table => new
                {
                    IlceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IlceAdi = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SehirID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ilceler", x => x.IlceID);
                    table.ForeignKey(
                        name: "FK_Ilceler_Sehirler_SehirID",
                        column: x => x.SehirID,
                        principalTable: "Sehirler",
                        principalColumn: "SehirID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_IlceID",
                table: "Etkinlikler",
                column: "IlceID");

            migrationBuilder.CreateIndex(
                name: "IX_Ilceler_SehirID",
                table: "Ilceler",
                column: "SehirID");

            migrationBuilder.AddForeignKey(
                name: "FK_Etkinlikler_Ilceler_IlceID",
                table: "Etkinlikler",
                column: "IlceID",
                principalTable: "Ilceler",
                principalColumn: "IlceID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etkinlikler_Ilceler_IlceID",
                table: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "Ilceler");

            migrationBuilder.DropTable(
                name: "Sehirler");

            migrationBuilder.DropIndex(
                name: "IX_Etkinlikler_IlceID",
                table: "Etkinlikler");

            migrationBuilder.DropColumn(
                name: "SeansAdi",
                table: "Seanslar");

            migrationBuilder.DropColumn(
                name: "KoltukNo",
                table: "Koltuklar");

            migrationBuilder.DropColumn(
                name: "IlceID",
                table: "Etkinlikler");
        }
    }
}
