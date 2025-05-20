using erp_server.Data;
using erp_server.Models;
using erp_server.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace erp_server.Services.Repositories
{
    public class TagService(AppDbContext context) : BaseService<Tag>(context)
    {
        private new readonly AppDbContext _context = context;
        // 新增 Tag
        // 新增 Tag，傳入 name 和 color
        public async Task<Tag> AddTagAsync(string name, ColorName color)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Tag name cannot be null or empty.");
            }

            try
            {
                // 創建新的 Tag 物件
                var newTag = new Tag
                {
                    Id = Guid.NewGuid(), // 使用 Guid 生成唯一 ID
                    Name = name,
                    Color = color,
                    Enable = true // 預設啟用
                };

                // 將新 Tag 加入資料庫
                _context.Tags.Add(newTag);
                await _context.SaveChangesAsync();

                return newTag; // 回傳新增的 Tag 物件
            }
            catch (Exception ex)
            {
                // 記錄錯誤（可選），並丟出異常
                throw new InvalidOperationException("An error occurred while saving the tag.", ex);
            }
        }

        // 獲取所有 Tag
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags
                .Include(tag => tag.MaterialTags)  // 如果需要關聯查詢，可根據需求調整
                .Include(tag => tag.ProductTags)   // 如果需要關聯查詢，可根據需求調整
                .ToListAsync();
        }
    }
}
