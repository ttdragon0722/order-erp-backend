using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace erp_server.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public bool Enable { get; set; } = true;

        [ForeignKey("TypeEntity")]
        public Guid? TypeId { get; set; }
        public virtual TypeEntity? Type { get; set; }

        public virtual ICollection<ProductMaterial>? ProductMaterials { get; set; }
    }
}
