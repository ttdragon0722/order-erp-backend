using erp_server.Models.Enums;

namespace erp_server.Dtos
{

    public class MaterialDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool Enable { get; set; }
        public StockStatus Stock { get; set; }
        public int? StockAmount { get; set; }
        public bool HasStock { get; set; }
        public List<TagDto> MaterialTags { get; set; } = [];
    }
    public class AddMaterialWithTagsDto
    {
        public string Name { get; set; } = string.Empty;
        public List<Guid> TagIds { get; set; } = [];
    }

    public class UpdateStockRequest
    {
        public Guid Id { get; set; }
        public StockStatus StockStatus { get; set; }  // ✅ 直接使用 enum
        public int? StockAmount { get; set; }
    }

    public class MaterialObj {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public StockStatus Stock { get; set; }
        public int? StockAmount { get; set; }
        public bool HasStock { get; set; }
    }

}
