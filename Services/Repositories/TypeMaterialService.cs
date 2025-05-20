using erp_server.Data;
using erp_server.Models;

namespace erp_server.Services.Repositories
{
    public class TypeMaterialsService(AppDbContext context) : BaseService<TypeMaterials>(context)
    {
        public bool AddMappings(Guid typeId, List<Guid> materialIds)
        {
            var mappings = materialIds.Select(materialId => new TypeMaterials
            {
                TypeEntityId = typeId,
                MaterialId = materialId
            }).ToList();

            _context.TypeMaterials.AddRange(mappings);
            return _context.SaveChanges() > 0;
        }

    }
}
