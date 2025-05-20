using System.ComponentModel.DataAnnotations;

namespace erp_server.Dtos
{
    public class AddClassificationDto
    {
        public string Name { get; set; } = null!;
        public bool Enable { get; set; }
    }

    public class UpdateSortOrderDto
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }  
    }
    public class ClassificationListDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public bool HasStock { get; set; }
    }
}
