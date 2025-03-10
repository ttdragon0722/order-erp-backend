using System.ComponentModel.DataAnnotations;


namespace erp_server.Models
{
    public class BusinessSettings
    {
        [Key]  // 主鍵
        public string Id { get; set; } = "default setting";
        public bool EnableOrdering { get; set; } = true;  // 點餐系統啟用
        public bool EnableDineIn { get; set; } = true;    // 啟用內用
        public bool EnableTakeout { get; set; } = true;   // 啟用外帶
        public bool EnableDelivery { get; set; } = true;  // 啟用外送

    }
}
