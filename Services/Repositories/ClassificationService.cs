using erp_server.Data;
using erp_server.Dtos;
using erp_server.Models;
using erp_server.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace erp_server.Services.Repositories
{
    public class ClassificationService(AppDbContext context, MaterialService _materialService) : BaseService<TypeEntity>(context)
    {
        private new readonly AppDbContext _context = context;
        private readonly MaterialService _materialService = _materialService;

        // 偵測是否存在
        public async Task<bool> TypeExistsAsync(Guid typeId)
        {
            return await _context.Set<TypeEntity>().AnyAsync(t => t.Id == typeId);
        }

        // 取得所有分類
        public async Task<List<TypeEntityDto>> GetAllClassificationsAsync()
        {
            var entities = await _context.Types
                .Include(t => t.TypeMaterials)
                .OrderBy(t => t.SortOrder)
                .ToListAsync();

            var allMaterialIds = entities
                .SelectMany(e => e.TypeMaterials?.Select(tm => tm.MaterialId) ?? [])
                .Distinct()
                .ToList();

            // 從 MaterialService 拿所有對應的資料
            var materials = await _materialService.GetMaterialsByIdsAsync(allMaterialIds);
            var materialDict = materials.ToDictionary(m => m.Id, m => m);

            // 組合 DTO
            var dtoList = entities.Select(entity => new TypeEntityDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Enable = entity.Enable,
                SortOrder = entity.SortOrder,
                TypeMaterials = entity.TypeMaterials?
                    .Select(tm => materialDict.TryGetValue(tm.MaterialId, out var mat) ? mat : null)
                    .OfType<TypeMaterial>()
                    .ToList() ?? []

            }).ToList();

            return dtoList;
        }
        public async Task<List<ClassificationListDto>> GetClassificationListAsync()
        {
            var entities = await _context.Types
                .Include(t => t.TypeMaterials)
                .Where(t => t.Enable)
                .OrderBy(t => t.SortOrder)
                .ToListAsync();

            var allMaterialIds = entities
                .SelectMany(e => e.TypeMaterials?.Select(tm => tm.MaterialId) ?? new Guid[0])
                .Distinct()
                .ToList();

            var materials = await _materialService.GetMaterialsByIdsAsync(allMaterialIds);
            var materialDict = materials.ToDictionary(m => m.Id, m => m);

            var dtoList = entities.Select(entity =>
            {
                bool hasStock = true;

                if (entity.TypeMaterials != null && entity.TypeMaterials.Count > 0)
                {
                    bool anyNoStock = entity.TypeMaterials.Any(tm =>
                        materialDict.TryGetValue(tm.MaterialId, out var mat) &&
                        (mat.Stock == StockStatus.None ||
                         (mat.Stock != StockStatus.Unlimited && (mat.StockAmount ?? 0) <= 0))
                    );

                    hasStock = !anyNoStock;
                }

                return new ClassificationListDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    HasStock = hasStock
                };
            }).ToList();

            return dtoList;
        }

        // 透過 ID 取得單一分類
        public async Task<TypeEntityDto?> GetClassificationByIdAsync(Guid id)
        {
            var entity = await _context.Types
                .Include(t => t.TypeMaterials)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entity == null) return null;

            var materialIds = entity.TypeMaterials?
                .Select(tm => tm.MaterialId)
                .Distinct()
                .ToList() ?? [];

            var materials = await _materialService.GetMaterialsByIdsAsync(materialIds);
            var materialDict = materials.ToDictionary(m => m.Id, m => m);

            return new TypeEntityDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Enable = entity.Enable,
                SortOrder = entity.SortOrder,
                TypeMaterials = entity.TypeMaterials?
                    .Select(tm => materialDict.TryGetValue(tm.MaterialId, out var mat) ? mat : null)
                    .OfType<TypeMaterial>()
                    .ToList() ?? []
            };
        }


        // 新增分類
        public async Task<TypeEntity> AddClassificationAsync(string name, bool enable)
        {
            // 計算當前最大 SortOrder 並加 1
            var maxSortOrder = await _context.Types
                .OrderByDescending(t => t.SortOrder)
                .Select(t => t.SortOrder)
                .FirstOrDefaultAsync();

            // 如果沒有分類，則設置排序為 1
            var newSortOrder = maxSortOrder + 1;

            // 創建新的分類
            var newType = new TypeEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Enable = enable,
                SortOrder = newSortOrder
            };

            // 新增分類到資料庫
            _context.Types.Add(newType);
            await _context.SaveChangesAsync();

            return newType;
        }


        // 修改分類
        public async Task<TypeEntity?> EditClassificationAsync(Guid id, string name, bool enable)
        {
            var existing = await _context.Types.FindAsync(id);
            if (existing == null)
                return null;

            existing.Name = name;
            existing.Enable = enable;

            _context.Types.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        // 修改分類名稱
        public async Task<TypeEntity?> UpdateClassificationNameAsync(Guid id, string newName)
        {
            var existing = await _context.Types.FindAsync(id);
            if (existing == null)
                return null;

            existing.Name = newName;
            _context.Types.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        // 修改分類啟用狀態
        public async Task<TypeEntity?> UpdateClassificationEnableStatusAsync(Guid id, bool enable)
        {
            var existing = await _context.Types.FindAsync(id);
            if (existing == null)
                return null;

            existing.Enable = enable;
            _context.Types.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<IEnumerable<TypeEntity>> UpdateSortOrderAsync(IEnumerable<UpdateSortOrderDto> updatedSortOrders)
        {
            foreach (var update in updatedSortOrders)
            {
                var entity = await _context.Types.FindAsync(update.Id);
                if (entity != null)
                {
                    entity.SortOrder = update.SortOrder;
                    _context.Types.Update(entity);
                }
            }

            await _context.SaveChangesAsync();
            return await _context.Types.OrderBy(t => t.SortOrder).ToListAsync(); // 根據 SortOrder 排序返回結果
        }


    }
}
