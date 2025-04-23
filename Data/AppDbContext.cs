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
        public DbSet<MaterialTags> MaterialTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ------  material --------
            modelBuilder.Entity<Material>()
                .Property(m => m.Stock)
                .HasConversion<int>();

            // ------  business setting ---------
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



            // ------  product material ---------
            // 設定product material 多對多關聯
            modelBuilder.Entity<ProductMaterial>()
                .HasKey(pm => new { pm.ProductId, pm.MaterialId });

            // 設定 product material 唯一值 和 外來鍵
            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.ProductMaterials)
                .HasForeignKey(pm => pm.ProductId);

            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Material)
                .WithMany(m => m.ProductMaterials)
                .HasForeignKey(pm => pm.MaterialId);


            // ------  user ---------

            // 設定 User.userId 為唯一
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId)
                .IsUnique();



            // ------  tag ---------
            // 設定 Tag.Name 為唯一
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // ----- material and tag ----
            modelBuilder.Entity<MaterialTags>()
            .HasKey(mt => new { mt.MaterialId, mt.TagId });

            modelBuilder.Entity<MaterialTags>()
                .HasOne(mt => mt.Material)
                .WithMany(m => m.MaterialTags)
                .HasForeignKey(mt => mt.MaterialId);

            modelBuilder.Entity<MaterialTags>()
                .HasOne(mt => mt.Tag)
                .WithMany(t => t.MaterialTags)
                .HasForeignKey(mt => mt.TagId);

        }
    }
}
