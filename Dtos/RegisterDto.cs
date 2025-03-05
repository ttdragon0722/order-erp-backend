namespace erp_server.Dtos
{
    public class RegisterDto
    {
        public required string UserId { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }
}
