namespace erp_server.Models
{
    public class ProductExcludedOption
    {
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid OptionId { get; set; }
        public Option? Option { get; set; }
    }
}
