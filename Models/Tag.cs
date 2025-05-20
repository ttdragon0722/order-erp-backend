using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using erp_server.Models.Enums;

namespace erp_server.Models
{
    // 用於原料分類 只有後臺會使用
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)] // 避免索引過長
        public required string Name { get; set; }
        public required ColorName Color { get; set; }
        public bool Enable { get; set; } = true;

        [JsonIgnore]
        public ICollection<MaterialTag>? MaterialTags { get; set; }
        
        [JsonIgnore]
        public ICollection<ProductTag>? ProductTags { get; set; }

    }
}
