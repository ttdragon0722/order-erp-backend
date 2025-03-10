using Microsoft.EntityFrameworkCore;
using erp_server.Models;

namespace erp_server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<BusinessSettings> BusinessSettings { get; set; }

        public DbSet<Material> Materials { get; set; }
        public DbSet<TypeEntity> Types { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<OptionTable> Options { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableOrdering)
                .HasComment("點餐系統啟用")
                .HasDefaultValue(true);  // 設置預設值為 true


            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableDineIn)
                .HasComment("啟用內用")
                .HasDefaultValue(true);  // 設置預設值為 true


            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableTakeout)
                .HasComment("啟用外帶")
                .HasDefaultValue(true);  // 設置預設值為 true


            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableDelivery)
                .HasComment("啟用外送")
                .HasDefaultValue(true);  // 設置預設值為 true


            // 設定多對多關聯
            modelBuilder.Entity<ProductMaterial>()
                .HasKey(pm => new { pm.ProductId, pm.MaterialId });


            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.ProductMaterials)
                .HasForeignKey(pm => pm.ProductId);

            // 
            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Material)
                .WithMany(m => m.ProductMaterials)
                .HasForeignKey(pm => pm.MaterialId);

            // 設定 User.userId 為唯一
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            // 設定 Tag.Name 為唯一
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}
