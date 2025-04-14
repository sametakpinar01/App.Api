using App.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data
{
    public class AppDbContext : DbContext
    {
        // AppDbContext sınıfı, Entity Framework Core ile veritabanı işlemlerini yönetir
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Students tablosunu temsil eden DbSet
        public DbSet<StudentEntity> Students { get; set; }

        // Veritabanı model yapılandırmaları
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Students tablosu için ek yapılandırmalar
            modelBuilder.Entity<StudentEntity>(entity =>
            {
                // Öğrenci numarasının benzersiz olması gerektiği belirtiliyor
                entity.HasIndex(e => e.No).IsUnique();

                // Sınıf alanı için varsayılan değer atanıyor
                entity.Property(e => e.Class).HasDefaultValue("Unknown");
            });

            // Gelecekte diğer tablolar için yapılandırmalar buraya eklenebilir
        }
    }
}
