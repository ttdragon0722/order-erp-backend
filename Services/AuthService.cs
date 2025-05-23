using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using erp_server.Models;
using Microsoft.IdentityModel.Tokens;

namespace erp_server.Services
{
    public class AuthService(IConfiguration config)
    {
        private readonly IConfiguration _config = config;

        /// <summary>
        /// 產生 JWT Token，根據不同角色帶入不同 Claim
        /// </summary>
        /// <param name="user">使用者物件</param>
        /// <param name="role">使用者角色，例如 "Customer", "Admin"</param>
        /// <returns>JWT Token 字串</returns>
        public string GenerateJwtToken(JwtUserInfo user)
        {
            var keyString = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Role, user.Role.ToString())
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

        public JwtUserInfo? DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims.ToList();

                var idClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (idClaim == null || nameClaim == null || roleClaim == null)
                {
                    return null;
                }

                return new JwtUserInfo
                {
                    Id = Guid.Parse(idClaim.Value),
                    Name = nameClaim.Value,
                    Role = Enum.Parse<Role>(roleClaim.Value)
                };
            }
            catch
            {
                return null;
            }
        }
    }
}