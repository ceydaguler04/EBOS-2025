using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBOS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GorselYoluTemiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EtkinlikTurleri",
                columns: table => new
                {
                    TurID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TurAdi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtkinlikTurleri", x => x.TurID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kampanyalar",
                columns: table => new
                {
                    KampanyaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KampanyaAdi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IndirimYuzdesi = table.Column<int>(type: "int", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kampanyalar", x => x.KampanyaID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdSoyad = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Eposta = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sifre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rol = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mekanlar",
                columns: table => new
                {
                    MekanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IlceID = table.Column<int>(type: "int", nullable: false),
                    MekanApiId = table.Column<int>(type: "int", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Raporlar",
                columns: table => new
                {
                    RaporID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RaporAdi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DosyaYolu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raporlar", x => x.RaporID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Salonlar",
                columns: table => new
                {
                    SalonID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SalonAdi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SatirSayisi = table.Column<int>(type: "int", nullable: false),
                    SutunSayisi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salonlar", x => x.SalonID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "koltuklar",
                columns: table => new
                {
                    KoltukID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KoltukNo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SalonID = table.Column<int>(type: "int", nullable: false),
                    Satir = table.Column<int>(type: "int", nullable: false),
                    Sutun = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_koltuklar", x => x.KoltukID);
                    table.ForeignKey(
                        name: "FK_koltuklar_Salonlar_SalonID",
                        column: x => x.SalonID,
                        principalTable: "Salonlar",
                        principalColumn: "SalonID",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Etkinlikler",
                columns: table => new
                {
                    EtkinlikID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    EtkinlikAdi = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Aciklama = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TurID = table.Column<int>(type: "int", nullable: false),
                    GorselYolu = table.Column<string>(type: "TEXT", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SureDakika = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Saat = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    IlceID = table.Column<int>(type: "int", nullable: true),
                    MekanID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etkinlikler", x => x.EtkinlikID);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_EtkinlikTurleri_TurID",
                        column: x => x.TurID,
                        principalTable: "EtkinlikTurleri",
                        principalColumn: "TurID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_Ilceler_IlceID",
                        column: x => x.IlceID,
                        principalTable: "Ilceler",
                        principalColumn: "IlceID");
                    table.ForeignKey(
                        name: "FK_Etkinlikler_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Etkinlikler_Mekanlar_MekanID",
                        column: x => x.MekanID,
                        principalTable: "Mekanlar",
                        principalColumn: "MekanID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Degerlendirmeler",
                columns: table => new
                {
                    YorumID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    EtkinlikID = table.Column<int>(type: "int", nullable: false),
                    Puan = table.Column<int>(type: "int", nullable: false),
                    Yorum = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Degerlendirmeler", x => x.YorumID);
                    table.ForeignKey(
                        name: "FK_Degerlendirmeler_Etkinlikler_EtkinlikID",
                        column: x => x.EtkinlikID,
                        principalTable: "Etkinlikler",
                        principalColumn: "EtkinlikID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Degerlendirmeler_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Seanslar",
                columns: table => new
                {
                    SeansID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SeansAdi = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EtkinlikID = table.Column<int>(type: "int", nullable: false),
                    SalonID = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Saat = table.Column<TimeSpan>(type: "time(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seanslar", x => x.SeansID);
                    table.ForeignKey(
                        name: "FK_Seanslar_Etkinlikler_EtkinlikID",
                        column: x => x.EtkinlikID,
                        principalTable: "Etkinlikler",
                        principalColumn: "EtkinlikID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Seanslar_Salonlar_SalonID",
                        column: x => x.SalonID,
                        principalTable: "Salonlar",
                        principalColumn: "SalonID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Biletler",
                columns: table => new
                {
                    BiletID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KullaniciID = table.Column<int>(type: "int", nullable: false),
                    SeansID = table.Column<int>(type: "int", nullable: true),
                    KoltukID = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    KampanyaUygulandiMi = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SatinAlmaTarihi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AktifMi = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EtkinlikID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biletler", x => x.BiletID);
                    table.ForeignKey(
                        name: "FK_Biletler_Etkinlikler_EtkinlikID",
                        column: x => x.EtkinlikID,
                        principalTable: "Etkinlikler",
                        principalColumn: "EtkinlikID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Biletler_Kullanicilar_KullaniciID",
                        column: x => x.KullaniciID,
                        principalTable: "Kullanicilar",
                        principalColumn: "KullaniciID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Biletler_Seanslar_SeansID",
                        column: x => x.SeansID,
                        principalTable: "Seanslar",
                        principalColumn: "SeansID");
                    table.ForeignKey(
                        name: "FK_Biletler_koltuklar_KoltukID",
                        column: x => x.KoltukID,
                        principalTable: "koltuklar",
                        principalColumn: "KoltukID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_EtkinlikID",
                table: "Biletler",
                column: "EtkinlikID");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_KoltukID",
                table: "Biletler",
                column: "KoltukID");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_KullaniciID",
                table: "Biletler",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_SeansID",
                table: "Biletler",
                column: "SeansID");

            migrationBuilder.CreateIndex(
                name: "IX_Degerlendirmeler_EtkinlikID",
                table: "Degerlendirmeler",
                column: "EtkinlikID");

            migrationBuilder.CreateIndex(
                name: "IX_Degerlendirmeler_KullaniciID",
                table: "Degerlendirmeler",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_IlceID",
                table: "Etkinlikler",
                column: "IlceID");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_KullaniciID",
                table: "Etkinlikler",
                column: "KullaniciID");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_MekanID",
                table: "Etkinlikler",
                column: "MekanID");

            migrationBuilder.CreateIndex(
                name: "IX_Etkinlikler_TurID",
                table: "Etkinlikler",
                column: "TurID");

            migrationBuilder.CreateIndex(
                name: "IX_Ilceler_SehirID",
                table: "Ilceler",
                column: "SehirID");

            migrationBuilder.CreateIndex(
                name: "IX_koltuklar_SalonID",
                table: "koltuklar",
                column: "SalonID");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_Eposta",
                table: "Kullanicilar",
                column: "Eposta",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seanslar_EtkinlikID",
                table: "Seanslar",
                column: "EtkinlikID");

            migrationBuilder.CreateIndex(
                name: "IX_Seanslar_SalonID",
                table: "Seanslar",
                column: "SalonID");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_KullaniciID",
                table: "Yorumlar",
                column: "KullaniciID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biletler");

            migrationBuilder.DropTable(
                name: "Degerlendirmeler");

            migrationBuilder.DropTable(
                name: "Kampanyalar");

            migrationBuilder.DropTable(
                name: "Raporlar");

            migrationBuilder.DropTable(
                name: "Yorumlar");

            migrationBuilder.DropTable(
                name: "Seanslar");

            migrationBuilder.DropTable(
                name: "koltuklar");

            migrationBuilder.DropTable(
                name: "Etkinlikler");

            migrationBuilder.DropTable(
                name: "Salonlar");

            migrationBuilder.DropTable(
                name: "EtkinlikTurleri");

            migrationBuilder.DropTable(
                name: "Ilceler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Mekanlar");

            migrationBuilder.DropTable(
                name: "Sehirler");
        }
    }
}
