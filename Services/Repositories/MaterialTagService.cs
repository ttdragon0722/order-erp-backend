using erp_server.Models;
using erp_server.Data;
using Microsoft.EntityFrameworkCore;
using erp_server.Dtos;

namespace erp_server.Services.Repositories
{
    public class MaterialTagService(AppDbContext context) : BaseService<MaterialTag>(context)
    {
        private new readonly AppDbContext _context = context;

        /// <summary>
        /// 根據 MaterialId，取得所有綁定的 Tag 資料，並轉換為 TagDto。
        /// </summary>
        public async Task<List<TagDto>> GetTagsByMaterialIdAsync(Guid materialId)
        {
            // 根據 MaterialId 查找所有綁定的 MaterialTag，並提取對應的 Tag 資料
            var materialTags = await _context.MaterialTags
                .Where(mt => mt.MaterialId == materialId)
                .Include(mt => mt.Tag) // 確保載入 Tag 資料
                .ToListAsync();

            // 將每個 MaterialTag 轉換為 TagDto
            var result = materialTags.Select(mt => new TagDto
            {
                Id = mt.Tag.Id,          // 取得 Tag 的 Id
                Name = mt.Tag.Name,      // 取得 Tag 的 Name
                Color = mt.Tag.Color     // 取得 Tag 的 Color
            }).ToList();

            return result;
        }

        public async Task<bool> AddMappings(Guid materialId, List<Guid> tags)
        {
            var mappings = tags.Select(tagId => new MaterialTag
            {
                MaterialId = materialId,
                TagId = tagId
            }).ToList();

            await _context.MaterialTags.AddRangeAsync(mappings);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
