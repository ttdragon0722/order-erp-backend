using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace erp_server.Models {
    public class OptionChildren
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; } 

        [JsonIgnore]
        public ICollection<Option>? Options { get; set; }
    }
}

