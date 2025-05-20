using System.Threading.Tasks;
using erp_server.Data;
using erp_server.Dtos;
using erp_server.Models;
using erp_server.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace erp_server.Services.Repositories
{
    public class MaterialService(AppDbContext context, MaterialTagService materialTagService) : BaseService<Material>(context)
    {
        private readonly MaterialTagService _materialTagService = materialTagService;


        // 這樣使用這條
        // if (!materialService.ValidateStock(material))
        // {
        //     return BadRequest("Invalid stock configuration");
        // }

        public bool ValidateStock(Material material)
        {
            if (material.Stock == StockStatus.Unlimited && material.StockAmount.HasValue)
            {
                return false; // 無限庫存時，StockAmount 應該為 NULL
            }
            if (material.Stock == StockStatus.Limited && (!material.StockAmount.HasValue || material.StockAmount <= 0))
            {
                return false; // 有限庫存時，StockAmount 應該為正數
            }
            return true;
        }

        public async Task<bool> UpdateStockAsync(Guid id, StockStatus stockStatus, int? stockAmount)
        {
            try
            {
                var material = await _context.Materials.FindAsync(id); // 假設資料表叫 Materials
                if (material == null) return false;

                if (stockStatus == StockStatus.Unlimited)
                {
                    material.Stock = StockStatus.Unlimited;
                    material.StockAmount = null;
                }
                else if (stockStatus == StockStatus.None)
                {
                    material.Stock = StockStatus.None;
                    material.StockAmount = null;
                }
                else if (stockStatus == StockStatus.Limited)
                {
                    if (stockAmount == null)
                        return false;

                    if (stockAmount == 0)
                    {
                        material.Stock = StockStatus.None;
                        material.StockAmount = null;
                    }
                    else
                    {
                        material.Stock = StockStatus.Limited;
                        material.StockAmount = stockAmount;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // 這邊可以記錄錯誤 log 或傳給 logging system
                Console.WriteLine($"更新庫存失敗: {ex.Message}");
                return false;
            }
        }

        public bool Drop(Guid id)
        {
            var material = _context.Materials.FirstOrDefault(m => m.Id == id);
            if (material == null) return false;

            _context.Materials.Remove(material);
            return _context.SaveChanges() > 0;
        }

        public async Task<List<MaterialDto>> GetAllMaterialsWithTagsAsync()
        {
            // 1. 讀取所有 Material 資料
            var materials = await _context.Materials
                .OrderBy(m => m.Name)
                .ToListAsync();

            var result = new List<MaterialDto>();

            foreach (var material in materials)
            {
                // 2. 用 material id 去拿綁定的 tags 資料
                var materialTagDtos = await _materialTagService.GetTagsByMaterialIdAsync(material.Id);

                // 3. 組合 MaterialDto，並將 Tag 轉換為 MaterialTag DTO
                var materialDto = new MaterialDto
                {
                    Id = material.Id,
                    Name = material.Name,
                    Enable = material.Enable,
                    Stock = material.Stock,
                    HasStock = material.HasStock,
                    StockAmount = material.StockAmount,

                    MaterialTags = materialTagDtos // 這裡直接把取得的 MaterialTagDto 放進去
                };

                result.Add(materialDto);
            }

            return result;
        }

        public async Task<List<TypeMaterial>> GetMaterialsByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Materials
                .Where(m => ids.Contains(m.Id))
                .Select(m => new TypeMaterial
                {
                    Id = m.Id,
                    Name = m.Name,
                    Stock = m.Stock,
                    StockAmount = m.StockAmount
                })
                .ToListAsync();
        }


        public Material AddMaterial(string name)
        {
            var material = new Material
            {
                Id = Guid.NewGuid(),
                Name = name,
                Enable = true,
                Stock = StockStatus.Unlimited,
                StockAmount = null
            };
            _context.Materials.Add(material);
            _context.SaveChanges();
            return material;
        }

        public async Task<MaterialDto> AddMaterialWithTags(string name, List<Guid> tagIds)
        {
            var newMaterial = this.AddMaterial(name);
            await _materialTagService.AddMappings(newMaterial.Id, tagIds);
            var newMaterialTags = await _materialTagService.GetTagsByMaterialIdAsync(newMaterial.Id);
            var newMaterialData = new MaterialDto
            {
                Id = newMaterial.Id,
                Name = newMaterial.Name,
                Enable = newMaterial.Enable,
                Stock = newMaterial.Stock,
                HasStock = newMaterial.HasStock,
                StockAmount = newMaterial.StockAmount,
                MaterialTags = newMaterialTags
            };

            return newMaterialData;
        }

        public bool UpdateMaterialName(Guid id, string newName)
        {
            var material = _context.Materials.Find(id);
            if (material == null) return false;
            material.Name = newName;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateMaterialEnable(Guid id, bool enable)
        {
            var material = _context.Materials.Find(id);
            if (material == null) return false;
            material.Enable = enable;
            _context.SaveChanges();
            return true;
        }
    }
}
