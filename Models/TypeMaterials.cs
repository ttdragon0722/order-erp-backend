namespace erp_server.Models
{
    public class TypeMaterials
    {
        public Guid TypeEntityId { get; set; }
        public TypeEntity? TypeEntity { get; set; }

        public Guid MaterialId { get; set; }
        public Material? Material { get; set; }
    }
}
