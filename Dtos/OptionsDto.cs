using erp_server.Models.Enums;

namespace erp_server.Dtos
{
    public class CreateOptionDto
    {
        public required string Name { get; set; }
        public double? Price { get; set; }
        public OptionGroupType Type { get; set; } = OptionGroupType.General;
        public List<string>? Children { get; set; }  // 只有 type = OptionGroupType.Single 時有此欄位
        public bool? Require { get; set; } = false;  // 只有 type = OptionGroupType.Single 時有此欄位
        public OptionDependDto? OptionDepends { get; set; }

    }
    public class CreateOptionRequest
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public OptionGroupType Type { get; set; } = OptionGroupType.General; // 根據你的 enum 設定
        public List<string>? Children { get; set; }
        public bool? Require { get; set; } = false;
        public Guid? OptionDepends { get; set; }
    }

    public class OptionChildDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class OptionDependDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }

    public class OptionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public OptionGroupType Type { get; set; }
        public OptionDependDto? OptionDepends { get; set; }

        public bool? Require { get; set; } // 僅 type == Single 時會有值
        public List<OptionChildDto>? Children { get; set; } // 同上
    }


}
