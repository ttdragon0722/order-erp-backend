using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using erp_server.Models.Enums; // 引入 Enum 命名空間

namespace erp_server.Models
{
    public class Material
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool Enable { get; set; } = true;

        public StockStatus Stock { get; set; } = StockStatus.Unlimited;

        public int? StockAmount { get; set; }

        public ICollection<MaterialTag>? MaterialTags { get; set; }

        public ICollection<ProductMaterial>? ProductMaterials { get; set; }

        public ICollection<Option>? Options { get; set; }

        public ICollection<TypeMaterials>? TypeMaterials { get; set; }

        [NotMapped]
        public bool IsUnlimited => Stock == StockStatus.Unlimited;

        [NotMapped]
        public bool HasStock => Stock == StockStatus.Unlimited || (Stock == StockStatus.Limited && StockAmount > 0);


    }
}
