using erp_server.Models.Enums;

namespace erp_server.Dtos {
    public class TagDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ColorName Color { get; set; }
    }

}
