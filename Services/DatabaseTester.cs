using erp_server.Data;
using Microsoft.EntityFrameworkCore;

namespace erp_server.Services
{
    public class DatabaseTester
    {
        private readonly AppDbContext _dbContext;

        public DatabaseTester(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void TestDatabaseConnection()
        {
            try
            {
                // 嘗試開啟資料庫連線
                _dbContext.Database.OpenConnection();
                Console.WriteLine("✅ 資料庫連線成功！");

                // 關閉資料庫連線
                _dbContext.Database.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ 無法連線到資料庫！");
                Console.WriteLine($"🔍 錯誤訊息: {ex.Message}");
            }
        }
    }

}