using erp_server.Models.Enums;

namespace erp_server.Dtos
{

    public class TypeEntityDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool Enable { get; set; }
        public int SortOrder { get; set; }
        public List<TypeMaterial> TypeMaterials { get; set; } = [];
    }

    public class TypeMaterial
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public StockStatus Stock { get; internal set; }
        public int? StockAmount { get; internal set; }
    }

    public class TypeMaterialMappingDto
    {
        public Guid TypeEntityId { get; set; }
        public List<Guid>? TypeMaterials { get; set; }
    }

    public class TagMaterialMappingDto
    {
        public Guid MaterialId { get; set; }
        public List<Guid>? Tags { get; set; }
    }
}