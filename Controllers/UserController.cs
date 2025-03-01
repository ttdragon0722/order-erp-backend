using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using erp_server.Data;
using erp_server.Dtos;

using System.Text;

[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UsersController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // 註冊 API
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
        if (existingUser != null)
            return BadRequest("帳號已存在");

        string salt, hashedPassword;
        hashedPassword = PasswordHelper.HashPassword(dto.Password, out salt);

        var user = new User
        {
            UserId = dto.UserId,
            Password = hashedPassword,
            Salt = salt,
            Name = dto.Name
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("註冊成功");
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserId),
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // 登入 API
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginRequest dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.UserId);
        if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.Salt, user.Password))
            return Unauthorized("帳號或密碼錯誤");

        var token = GenerateJwtToken(user);

        // 🔹 設定 HttpOnly Cookie
        Response.Cookies.Append("auth_token", token, new CookieOptions
        {
            HttpOnly = true, // JS 不能存取，防止 XSS
            Secure = true, // 只有 HTTPS 可以傳送
            SameSite = SameSiteMode.Strict, // 防止 CSRF
            Expires = DateTime.UtcNow.AddDays(7) // 7 天後過期
        });

        return Ok(new { message = "登入成功", userId = user.Id });
    }
}
