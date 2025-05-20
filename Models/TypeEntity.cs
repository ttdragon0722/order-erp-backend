using System.ComponentModel.DataAnnotations;

namespace erp_server.Models
{
    public class TypeEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "類別名稱為必填")]
        public required string Name { get; set; }
        public bool Enable { get; set; } = true;
        public int SortOrder { get; set; } 
        public ICollection<Product>? Products { get; set; }
        public ICollection<TypeMaterials>? TypeMaterials { get; set; } // 一個 TypeEntity 可以對應多個 TypeMaterial
        public ICollection<TypeOption>? TypeOptions { get; set; }

    }
}
