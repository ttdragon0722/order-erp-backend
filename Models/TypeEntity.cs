using System.ComponentModel.DataAnnotations;

namespace erp_server.Models
{
    public class TypeEntity
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool Enable { get; set; } = true;
    }
}
