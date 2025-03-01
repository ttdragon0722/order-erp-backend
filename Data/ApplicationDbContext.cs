using Microsoft.EntityFrameworkCore;
using erp_server.Models; // 假設你的模型放在 Models 目錄

namespace erp_server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // 在這裡進行 Fluent API 設定 (例如 Table Mapping)
        }
    }
}
