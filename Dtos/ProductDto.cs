namespace erp_server.Dtos
{
    // 建立產品用 DTO
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid TypeId { get; set; }
        public bool Enable { get; set; } = true;

        public Guid[]? Depends { get; set; }
    }

    public class ProductObj
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<MaterialObj>? Depends { get; set; } = [];
        public bool HasStock { get; set; }

    }

    public class ProductListDto
    {
        public Guid TypeId { get; set; }
        public string TypeName { get; set; }

        public MaterialObj Depend { get; set; }
        public bool HasStock { get; set; }

        public List<ProductObj> Products { get; set; } = [];
    }

    public class ProductCart
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<MaterialObj>? Depends { get; set; } = [];
        public OptionResponse[] Options { get; set; } = [];

    }
}