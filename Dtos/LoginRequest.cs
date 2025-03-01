using System.ComponentModel.DataAnnotations;

namespace erp_server.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
