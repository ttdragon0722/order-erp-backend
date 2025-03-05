using System.ComponentModel.DataAnnotations;

namespace erp_server.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();  // UUID 主鍵

        [Required]
        [MaxLength(255)]
        public required string UserId { get; set; }  // 帳號，唯一

        [Required]
        [MaxLength(255)]
        public required string Password { get; set; }  // 加密後的密碼 (BCrypt)

        [Required]
        [MaxLength(255)]
        public required string Salt { get; set; }  // Salt 值

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }  // 使用者名稱

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "User";  // 角色 (Admin/User)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // 建立時間
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // 更新時間

        public bool IsActive { get; set; } = true;  // 帳號是否啟用
    }
}
