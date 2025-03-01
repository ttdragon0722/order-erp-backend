using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();  // UUID 主鍵

    [Required]
    [MaxLength(255)]
    public string UserId { get; set; }  // 帳號，唯一

    [Required]
    [MaxLength(255)]
    public string Password { get; set; }  // 加密後的密碼 (BCrypt)

    [Required]
    [MaxLength(255)]
    public string Salt { get; set; }  // Salt 值

    [Required]
    [MaxLength(255)]
    public string Name { get; set; }  // 使用者名稱

    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = "User";  // 角色 (Admin/User)

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // 建立時間
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  // 更新時間

    public bool IsActive { get; set; } = true;  // 帳號是否啟用
}
