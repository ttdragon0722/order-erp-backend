namespace erp_server.Models
{
    public class ProductTag
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
