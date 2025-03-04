using Microsoft.AspNetCore.Mvc;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using erp_server.Data;
using erp_server.Dtos;

using System.Text;

// repository
using erp_server.Services.Repositories;

[ApiController]
[Route("api")]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserService _userService;


    public UsersController(UserService userService, IConfiguration config)
    {
        _config = config;
        _userService = userService;
    }

    /// <summary>
    /// 註冊新使用者
    /// </summary>
    /// <param name="dto">使用者註冊資料</param>
    /// <returns>註冊成功或失敗的訊息</returns>
    /// <response code="200">註冊成功</response>
    /// <response code="400">帳號已存在</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto dto)
    {
        var existingUser = await _userService.GetByUserIdAsync(dto.UserId);
        if (existingUser != null)
            return StatusCode(StatusCodes.Status409Conflict, new ApiResponse<object>
            {
                Success = false,
                Message = "帳號已存在"
            });

        string hashedPassword = PasswordHelper.HashPassword(dto.Password, out string salt);

        var user = new User
        {
            UserId = dto.UserId,
            Password = hashedPassword,
            Salt = salt,
            Name = dto.Name
        };

        await _userService.Register(user);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "註冊成功"
        });

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
        var user = await _userService.GetByUserIdAsync(dto.UserId);
        if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.Salt, user.Password))
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "帳號或密碼錯誤"
            });
        }

        var token = GenerateJwtToken(user);

        // 🔹 設定 HttpOnly Cookie
        Response.Cookies.Append("auth_token", token, new CookieOptions
        {
            HttpOnly = true, // JS 不能存取，防止 XSS
            Secure = true, // 只有 HTTPS 可以傳送
            SameSite = SameSiteMode.Strict, // 防止 CSRF
            Expires = DateTime.UtcNow.AddDays(7) // 7 天後過期
        });
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "登入成功",
            Data = new
            {
                UserId = user.Id
            }
        });

    }

    // 登出 API
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Append("auth_token", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1) // 設定過去的時間，使 Cookie 失效
        });

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "登出成功"
        });
    }
}
