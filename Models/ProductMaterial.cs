using System.ComponentModel.DataAnnotations.Schema;

namespace erp_server.Models
{
    public class ProductMaterial
    {
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Material")]
        public Guid MaterialId { get; set; }
        public virtual Material Material { get; set; }
    }
}
