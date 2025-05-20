using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace erp_server.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; } = 0;
        public bool Enable { get; set; } = true;


        public Guid TypeId { get; set; }
        
        [ForeignKey("TypeId")]
        public TypeEntity? Type { get; set; }
        public ICollection<ProductTag>? ProductTags { get; set; }
        public ICollection<ProductMaterial>? ProductMaterials { get; set; }

        public ICollection<ProductExcludedOption>? ExcludedOptions { get; set; }

        public ICollection<ProductOption>? ProductOptions { get; set; }
    }
}
