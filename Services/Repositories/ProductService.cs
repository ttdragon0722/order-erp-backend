using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using erp_server.Data;
using erp_server.Dtos;
using erp_server.Models;
using erp_server.Models.Enums;

namespace erp_server.Services.Repositories
{
    public class ProductService(
        AppDbContext context,
        ClassificationService classificationService,
        OptionService optionService
    ) : BaseService<Product>(context)
    {
        private new readonly AppDbContext _context = context;
        private readonly ClassificationService _classificationService = classificationService;

        private readonly OptionService _optionService = optionService;

        // 建立product
        public async Task<Product> CreateAsync(string name, decimal price, Guid typeId, bool enable = true, Guid[]? materialIds = null)
        {
            var typeExists = await _classificationService.TypeExistsAsync(typeId);
            if (!typeExists)
            {
                throw new ArgumentException("無效的 TypeId，找不到對應的 TypeEntity");
            }

            // 若有 materials，先查出資料庫中有的那些
            List<ProductMaterial>? productMaterials = null;
            if (materialIds != null && materialIds.Length != 0)
            {
                var validMaterialIds = await _context.Set<Material>()
                    .Where(m => materialIds.Contains(m.Id))
                    .Select(m => m.Id)
                    .ToListAsync();

                productMaterials = [.. validMaterialIds.Select(mid => new ProductMaterial
                {
                    MaterialId = mid
                })];
            }

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                Price = price,
                Enable = enable,
                TypeId = typeId,
                ProductMaterials = productMaterials
            };

            await _dbSet.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<List<ProductListDto>> GetProductList()
        {
            var products = await _context.Products
                .Include(p => p.Type)
                .Include(p => p.ProductMaterials!)
                    .ThenInclude(pm => pm.Material)
                .Where(p => p.Enable)
                .ToListAsync();

            var grouped = products
                .GroupBy(p => new
                {
                    p.TypeId,
                    TypeName = p.Type != null ? p.Type.Name : "未分類",
                    SortOrder = p.Type?.SortOrder ?? int.MaxValue
                })
                .OrderBy(g => g.Key.SortOrder)
                .Select(g =>
                {
                    var productList = g.Select(p =>
                    {
                        var depends = p.ProductMaterials?
                            .Where(pm => pm.Material != null)
                            .Select(pm =>
                            {
                                var m = pm.Material!;
                                return new MaterialObj
                                {
                                    Id = m.Id,
                                    Name = m.Name,
                                    Stock = m.Stock,
                                    StockAmount = m.StockAmount,
                                    HasStock = m.Stock == StockStatus.Unlimited ||
                                               (m.Stock == StockStatus.Limited && m.StockAmount > 0)
                                };
                            }).ToList() ?? [];

                        return new ProductObj
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            Depends = depends,
                            HasStock = depends.All(m => m.HasStock)
                        };
                    }).ToList();

                    // 統整出所有產品的原料，用 ID 做 distinct，簡單抓第一個
                    var allDepends = productList
                        .SelectMany(p => p.Depends)
                        .GroupBy(m => m.Id)
                        .Select(g => g.First())
                        .ToList();

                    // 判斷這個群組的 HasStock 是不是所有產品都有庫存
                    var groupHasStock = productList.All(p => p.HasStock);

                    return new ProductListDto
                    {
                        TypeId = g.Key.TypeId,
                        TypeName = g.Key.TypeName,
                        Products = productList,
                        Depend = allDepends.FirstOrDefault(), // 如果你只要顯示一個代表用這個
                        HasStock = groupHasStock
                    };
                })
                .ToList();

            return grouped;
        }
        public async Task<ProductCart?> GetProductCartByIdAsync(Guid productId)
        {
            // 1. 取產品（含 Type，但不 include ProductMaterials）
            var product = await _context.Products
                .Include(p => p.Type)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return null;
            }

            // 2. 取 ProductMaterials → Depends
            var productMaterialEntities = await _context.ProductMaterials
                .Where(pm => pm.ProductId == productId)
                .Include(pm => pm.Material)
                .ToListAsync();

            var depends = productMaterialEntities
                .Where(pm => pm.Material != null)
                .Select(pm => new MaterialObj
                {
                    Id = pm.MaterialId,
                    Name = pm.Material!.Name
                })
                .ToList();

            // 3. 加入從 TypeMaterials 找到的 Material（使用 TypeId）
            var typeMaterialEntities = await _context.TypeMaterials
                .Where(tm => tm.TypeEntityId == product.TypeId)
                .Include(tm => tm.Material)
                .ToListAsync();

            var typeDepends = typeMaterialEntities
                .Where(tm => tm.Material != null)
                .Select(tm => new MaterialObj
                {
                    Id = tm.MaterialId,
                    Name = tm.Material!.Name
                });

            // 合併（避免重複 MaterialId）
            var existingMaterialIds = depends.Select(d => d.Id).ToHashSet();
            depends.AddRange(typeDepends.Where(d => !existingMaterialIds.Contains(d.Id)));


            // 4. 取 options
            var availableOptions = await GetAvailableOptionsByProductIdAsync(productId);

            // 5. 封裝成 ProductCart
            var productCart = new ProductCart
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Depends = depends,
                Options = (availableOptions ?? []).ToArray()
            };

            return productCart;
        }


        private async Task<List<OptionResponse>> GetAvailableOptionsByProductIdAsync(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null) return [];

            // ✅ 從 TypeOption 表取得對應的 OptionIds
            var allOptionIds = await _context.TypeOptions
                .Where(to => to.TypeId == product.TypeId)
                .Select(to => to.OptionId)
                .ToListAsync();

            // ❌ 要排除的 OptionIds（針對此產品禁用的）
            var excludedOptionIds = await _context.ProductExcludedOptions
                .Where(e => e.ProductId == productId)
                .Select(e => e.OptionId)
                .ToListAsync();

            // ✅ 計算可用的 OptionIds
            var availableOptionIds = allOptionIds.Except(excludedOptionIds).ToArray();

            // ✅ 用 option service 取出詳細資料
            var availableOptions = await _optionService.GetOptionsByIdsAsync(availableOptionIds);

            return availableOptions;
        }
    }
}
