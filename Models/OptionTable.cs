using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_server.Models
{
    public class OptionTable
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Material")]
        public Guid? MaterialId { get; set; }
        public virtual Material Material { get; set; }

        [ForeignKey("TypeEntity")]
        public Guid? TypeId { get; set; }
        public virtual TypeEntity Type { get; set; }

        [ForeignKey("Product")]
        public Guid? ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
