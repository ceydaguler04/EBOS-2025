using Microsoft.EntityFrameworkCore;
using EBOS.Entities;

namespace EBOS.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<EtkinlikTuru> EtkinlikTurleri { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Koltuk> Koltuklar { get; set; }
        public DbSet<Seans> Seanslar { get; set; }
        public DbSet<Bilet> Biletler { get; set; }
        public DbSet<Kampanya> Kampanyalar { get; set; }
        public DbSet<Degerlendirme> Degerlendirmeler { get; set; }
        public DbSet<Rapor> Raporlar { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }
        public DbSet<Ilce> Ilceler { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Buraya MySQL bağlantı cümleni yazacaksın
            optionsBuilder.UseMySql(
                "server=localhost;database=EBOSDB;user=root;password=Sibel8181.;",
                new MySqlServerVersion(new Version(8, 0, 36))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Benzersiz E-posta (önceden hatırlattığın)
            modelBuilder.Entity<Kullanici>()
                .HasIndex(k => k.Eposta)
                .IsUnique();

            modelBuilder.Entity<Ilce>()
       .HasOne(i => i.Sehir)
       .WithMany(s => s.Ilceler)
       .HasForeignKey(i => i.SehirID)
       .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
