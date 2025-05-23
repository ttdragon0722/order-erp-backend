using System;
using System.ComponentModel.DataAnnotations;

namespace erp_server.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string LineUserId { get; set; } = null!; // 從 LINE 拿到的 userId，唯一識別

        [Required]
        public string DisplayName { get; set; } = null!;

        public string? PictureUrl { get; set; }

        public string? Email { get; set; } // 需取得 email scope 才會有值

        public string? PhoneNumber { get; set; } // 若後續收集用戶電話

        public string? CustomNote { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
        public string AccessToken { get; set; } = null!;
    }
}
