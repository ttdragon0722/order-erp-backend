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

        public virtual ICollection<ProductMaterial>? ProductMaterials { get; set; } = new List<ProductMaterial>();
        public virtual ICollection<MaterialTags> MaterialTags { get; set; } = new List<MaterialTags>();


        
        [NotMapped]
        public bool IsUnlimited => Stock == StockStatus.Unlimited;

        [NotMapped]
        public bool HasStock => Stock == StockStatus.Limited && StockAmount > 0;


    }
}
