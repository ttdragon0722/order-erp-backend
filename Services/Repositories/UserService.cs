using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using erp_server.Data;
using erp_server.Models;

namespace erp_server.Services.Repositories
{
    public class UserService : BaseService<User>
    {
        public UserService(AppDbContext context) : base(context) { }

        /// <summary>
        /// 註冊新使用者
        /// </summary>
        public async Task Register(User newUser)
        {
            await this.Insert(newUser);
        }

        /// <summary>
        /// 依據 UserId 查詢使用者
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>回傳符合條件的使用者，若無則為 null</returns>
        public async Task<User?> GetByUserIdAsync(string userId)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }
}
