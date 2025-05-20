using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace erp_server.Models
{
    public class OptionRadio
    {
        public Guid OptionId { get; set; }

        [ForeignKey("OptionId")]
        public Option? Option { get; set; }

        public Guid ChildrenId { get; set; }

        [ForeignKey("ChildrenId")]
        public OptionChildren? OptionChildren { get; set; }
    }
}
