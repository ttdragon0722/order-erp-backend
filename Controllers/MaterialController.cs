using Microsoft.AspNetCore.Mvc;

using erp_server.Services.Repositories;
using erp_server.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace erp_server.Controllers
{
    [ApiController]
    [Route("api")]
    public class MaterialController : ControllerBase
    {
        private readonly MaterialService _materialService;

        public MaterialController(MaterialService materialService)
        {
            _materialService = materialService;
        }

        [Authorize]
        [HttpGet("getMaterials")]
        public async Task<IActionResult> GetMaterials()
        {
            if (Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                Console.WriteLine($"🔹 Received Authorization Header: {authHeader}");
            }
            else
            {
                Console.WriteLine("⚠️ No Authorization Header received.");
            }
            var materialData = await _materialService.GetAll();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "取得原料",
                Data = materialData
            });
        }
    }
}
