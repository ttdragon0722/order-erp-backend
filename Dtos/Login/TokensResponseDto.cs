namespace erp_server.Dtos
{
    public class TokensResponseDto
    {
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public string Refresh_token { get; set; }
        public int Expires_in { get; set; }
        public string Scope { get; set; }
        public string? Id_token { get; set; }
        public void Log()
        {
            Console.WriteLine("==== Tokens Response ====");
            Console.WriteLine($"Access Token : {Access_token}");
            Console.WriteLine($"Token Type   : {Token_type}");
            Console.WriteLine($"Refresh Token: {Refresh_token}");
            Console.WriteLine($"Expires In   : {Expires_in}");
            Console.WriteLine($"Scope        : {Scope}");
            Console.WriteLine($"ID Token     : {Id_token}");
            Console.WriteLine("==========================");
        }
    }

    public class UserProfileDto
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string PictureUrl { get; set; }
        public string? StatusMessage { get; set; }
    }
    public class MeDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public string LineUserId { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}