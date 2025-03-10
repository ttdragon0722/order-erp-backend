using System.ComponentModel.DataAnnotations;

namespace erp_server.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(100)] // 避免索引過長
        public required string Name { get; set; }
        
    }
}
