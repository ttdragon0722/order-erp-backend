using erp_server.Services;
using erp_server.Dtos;
using Microsoft.AspNetCore.Mvc;
using erp_server.Services.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace erp_server.Controllers
{
    [Route("api/line")]
    [ApiController]
    public class LineLoginController(IConfiguration configuration, CustomerService customerService, AuthService authService) : ControllerBase
    {
        private readonly LineLoginService _lineLoginService = new(configuration);
        private readonly CustomerService _customerService = customerService;
        private readonly AuthService _authService = authService;

        [HttpGet("url")]
        public string GetLoginUrl()
        {
            return _lineLoginService.GetLoginUrl();
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("code is missing");
            }

            // 拿code換token
            var tokens = await _lineLoginService.GetTokensByAuthToken(code, configuration["BaseUrl"] + "/api/line/callback");
            tokens.Log();

            // 取得使用者資料
            var profile = await _lineLoginService.GetUserProfileByAccessToken(tokens.Access_token);

            // 檢查帳號是否存在
            var customer = await _customerService.GetByLineUserIdAsync(profile.UserId);
            if (customer == null)
            {
                // 新使用者
                customer = await _customerService.CreateCustomerAsync(profile.UserId, profile.DisplayName, profile.PictureUrl,tokens.Access_token);

            }
            else
            {
                // 已存在就更新登入時間
                await _customerService.UpdateLastLoginAsync(customer);
            }

            var jwtUser = new JwtUserInfo
            {
                Id = customer.Id,
                Name = customer.DisplayName,
                Role = Role.Customer
            };

            var token = _authService.GenerateJwtToken(jwtUser);
            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            return Redirect("/order");
        }

        [HttpGet("profile/{accessToken}")]
        public async Task<UserProfileDto> GetUserProfileByAccessToken(string accessToken)
        {
            return await _lineLoginService.GetUserProfileByAccessToken(accessToken);
        }

        [HttpGet("me")]
        public async Task<ActionResult<MeDto>> GetCurrentUser()
        {
            var token = Request.Cookies["auth_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token 不存在");
            }

            var jwtUser = _authService.DecodeJwtToken(token);
            if (jwtUser == null)
            {
                return Unauthorized("無效的 Token");
            }

            var customer = await _customerService.GetByIdAsync(jwtUser.Id);
            if (customer == null)
            {
                return NotFound("找不到使用者");
            }

            var dto = new MeDto
            {
                Id = customer.Id,
                DisplayName = customer.DisplayName,
                PictureUrl = customer.PictureUrl,
                LineUserId = customer.LineUserId,
                Role = jwtUser.Role.ToString()
            };

            return Ok(dto);
        }

    }
}