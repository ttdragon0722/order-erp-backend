using Microsoft.AspNetCore.Mvc;
using erp_server.Dtos;

[ApiController]
[Route("api/logindebug")]
public class LoginController : ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromForm] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request.UserId == "admin" && request.Password == "123456")
        {
            return Ok(new { message = "Login successful", token = "your_jwt_token_here" });
        }

        return Unauthorized(new { message = "Invalid username or password" });
    }
}
