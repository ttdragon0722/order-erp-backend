using erp_server.Data;
using erp_server.Models;
using erp_server.Models.Enums;

namespace erp_server.Services.Repositories
{
    public class MaterialService(AppDbContext context) : BaseService<Material>(context)
    {
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

    }
}
