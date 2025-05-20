using Microsoft.EntityFrameworkCore;
using erp_server.Models;

namespace erp_server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<BusinessSettings> BusinessSettings { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<TypeEntity> Types { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<TypeOption> TypeOptions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MaterialTag> MaterialTags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<ProductExcludedOption> ProductExcludedOptions { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<TypeMaterials> TypeMaterials { get; set; }
        public DbSet<OptionChildren> OptionChildren { get; set; }
        public DbSet<OptionRadio> OptionRadios { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // ------ BusinessSettings -------
            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableOrdering)
                .HasComment("點餐系統啟用")
                .HasDefaultValue(true);

            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableDineIn)
                .HasComment("啟用內用")
                .HasDefaultValue(true);

            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableTakeout)
                .HasComment("啟用外帶")
                .HasDefaultValue(true);

            modelBuilder.Entity<BusinessSettings>()
                .Property(b => b.EnableDelivery)
                .HasComment("啟用外送")
                .HasDefaultValue(true);

            // ------ ProductMaterial 多對多關聯 -------
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

            // ------ User -------
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId)
                .IsUnique();

            // ------ Tag -------
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // ------ MaterialTag 多對多關聯 -------
            modelBuilder.Entity<MaterialTag>()
                .HasKey(mt => new { mt.MaterialId, mt.TagId });

            modelBuilder.Entity<MaterialTag>()
                .HasOne(mt => mt.Material)
                .WithMany(m => m.MaterialTags)
                .HasForeignKey(mt => mt.MaterialId);

            modelBuilder.Entity<MaterialTag>()
                .HasOne(mt => mt.Tag)
                .WithMany(t => t.MaterialTags)
                .HasForeignKey(mt => mt.TagId);

            // ------ TypeOption 多對多關聯 -------
            modelBuilder.Entity<TypeOption>()
                .HasKey(to => new { to.TypeId, to.OptionId });

            // ------ ProductTag 多對多關聯 -------
            modelBuilder.Entity<ProductTag>()
                .HasKey(pt => new { pt.ProductId, pt.TagId });

            // ------ ProductExcludedOption 多對多關聯 -------
            modelBuilder.Entity<ProductExcludedOption>()
                .HasKey(peo => new { peo.ProductId, peo.OptionId });

            // ------ ProductOption 多對多關聯 -------
            modelBuilder.Entity<ProductOption>()
                .HasKey(po => new { po.ProductId, po.OptionId });

            // ------ Option 與 Material (nullable 外鍵) -------
            modelBuilder.Entity<Option>()
                .HasOne(o => o.Material)
                .WithMany(m => m.Options)
                .HasForeignKey(o => o.Depend)
                .OnDelete(DeleteBehavior.Restrict);


            // 設定 TypeMaterial 表的複合主鍵
            modelBuilder.Entity<TypeMaterials>()
                .HasKey(tm => new { tm.TypeEntityId, tm.MaterialId });

            // 設定 TypeMaterials 表的外鍵關聯
            modelBuilder.Entity<TypeMaterials>()
                .HasOne(tm => tm.TypeEntity)
                .WithMany(te => te.TypeMaterials)
                .HasForeignKey(tm => tm.TypeEntityId);

            modelBuilder.Entity<TypeMaterials>()
                .HasOne(tm => tm.Material)
                .WithMany(m => m.TypeMaterials)
                .HasForeignKey(tm => tm.MaterialId);

            // Option Radio
            modelBuilder.Entity<OptionRadio>()
                .HasKey(or => new { or.OptionId, or.ChildrenId });

        }
    }
}
