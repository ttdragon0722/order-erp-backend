using BCrypt.Net;

public static class PasswordHelper
{
    public static string HashPassword(string password, out string salt)
    {
        salt = BCrypt.Net.BCrypt.GenerateSalt();  // 產生隨機 Salt
        return BCrypt.Net.BCrypt.HashPassword(password + salt);  // 雜湊加密
    }

    public static bool VerifyPassword(string inputPassword, string salt, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(inputPassword + salt, storedHash);
    }
}