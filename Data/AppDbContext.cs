using Microsoft.EntityFrameworkCore;
using erp_server.Models; // 假設你的模型放在 Models 目錄

namespace erp_server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // public DbSet<Product> Products { get; set; }
        // public DbSet<Material> Materials { get; set; }
        // public DbSet<Type> Types { get; set; }
        // public DbSet<OptionTable> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 在這裡進行 Fluent API 設定 (例如 Table Mapping)
        }
    }
}
