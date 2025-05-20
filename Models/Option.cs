using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using erp_server.Models.Enums; // 引入 Enum 命名空間

namespace erp_server.Models {
    public class Option
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public double Price { get; set; } = 0;

        public Guid? Depend { get; set; }

        public OptionGroupType Type { get; set; } = 0;

        public bool Require {get; set;} = false;

        [ForeignKey("Depend")]
        public Material? Material { get; set; }

        [JsonIgnore]
        public ICollection<ProductOption>? ProductOptions { get; set; }

        [JsonIgnore]
        public ICollection<ProductExcludedOption>? ExcludedOptions { get; set; }

        [JsonIgnore]
        public ICollection<TypeOption>? TypeOptions { get; set; }
        [JsonIgnore]
        public ICollection<OptionRadio>? Radios { get; set; }

    }
}
