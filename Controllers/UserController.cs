using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using erp_server.Dtos;
using erp_server.Services.Repositories;
using erp_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using erp_server.Services;

namespace erp_server.Controllers
{
    /// <summary>
    /// 負責處理使用者相關的 API，例如註冊、登入和登出。
    /// </summary>
    [ApiController]
    [Route("api")]
    public class UsersController(UserService userService,AuthService authService, IConfiguration config, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IConfiguration _config = config;   // 取得應用程式設定，例如 JWT 金鑰
        private readonly UserService _userService = userService;  // 使用者服務，負責資料庫存取
        private readonly AuthService _authService = authService;  // 使用者服務，負責資料庫存取
        private readonly ILogger<UsersController> _logger = logger;  // 日誌記錄，記錄錯誤訊息

        /// <summary>
        /// 註冊新使用者
        /// </summary>
        /// <param name="dto">使用者註冊資訊</param>
        /// <returns>成功時回傳 200 OK，帳號已存在時回傳 409，伺服器錯誤回傳 500</returns>
        /// <response code="200">註冊成功</response>
        /// <response code="409">帳號已存在</response>
        /// <response code="500">伺服器內部錯誤</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            try
            {
                var existingUser = await _userService.GetByUserIdAsync(dto.UserId);
                if (existingUser != null)
                    return StatusCode(StatusCodes.Status409Conflict);

                string hashedPassword = PasswordHelper.HashPassword(dto.Password, out string salt);

                var user = new User
                {
                    UserId = dto.UserId,
                    Password = hashedPassword,
                    Salt = salt,
                    Name = dto.Name
                };

                await _userService.Register(user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// 產生 JWT Token，讓使用者登入後可以驗證身份
        /// </summary>
        /// <param name="user">使用者物件</param>
        /// <returns>JWT Token 字串</returns>
        private string GenerateJwtToken(User user)
        {
            var keyString = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // 使用者 ID
                new Claim(ClaimTypes.Name, user.UserId), // 使用者帳號
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

        /// <summary>
        /// 使用者登入，驗證帳號與密碼，並回傳 JWT Token
        /// </summary>
        /// <param name="dto">使用者登入資訊</param>
        /// <returns>成功時回傳 204 No Content，失敗時回傳 401 或 500</returns>
        /// <response code="204">登入成功</response>
        /// <response code="401">帳號或密碼錯誤</response>
        /// <response code="500">伺服器內部錯誤</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromForm] LoginRequest dto)
        {
            try
            {
                var user = await _userService.GetByUserIdAsync(dto.UserId);
                if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.Salt, user.Password))
                    return Unauthorized();

                var token = _authService.GenerateJwtToken(new JwtUserInfo
                {
                    Id = user.Id,
                    Name = user.UserId,
                    Role = Role.Admin
                });

                Response.Cookies.Append("auth_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登入時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// 使用者登出，清除 JWT Token Cookie
        /// </summary>
        /// <returns>成功時回傳 204 No Content，失敗時回傳 500</returns>
        /// <response code="204">登出成功</response>
        /// <response code="500">伺服器內部錯誤</response>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Append("auth_token", "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddDays(-1)
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登出時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
