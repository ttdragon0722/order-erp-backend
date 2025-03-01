using Microsoft.EntityFrameworkCore;
using erp_server.Models;

namespace erp_server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Material> Materials { get; set; }
        public DbSet<TypeEntity> Types { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<OptionTable> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 設定多對多關聯
            modelBuilder.Entity<ProductMaterial>()
                .HasKey(pm => new { pm.ProductId, pm.MaterialId });

            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.ProductMaterials)
                .HasForeignKey(pm => pm.ProductId);

            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Material)
                .WithMany(m => m.ProductMaterials)
                .HasForeignKey(pm => pm.MaterialId);
        }
    }
}
