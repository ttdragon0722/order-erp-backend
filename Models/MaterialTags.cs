using System.ComponentModel.DataAnnotations.Schema;

namespace erp_server.Models
{
    public class MaterialTags
    {
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; } = null!;

        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; } = null!;
    }
}
