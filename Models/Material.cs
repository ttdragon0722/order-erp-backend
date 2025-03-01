using System.ComponentModel.DataAnnotations;

namespace erp_server.Models
{
    public class Material
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; } = true;
        public bool Stock { get; set; } = false;
        public int? StockAmount { get; set; }

        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }
    }
}
