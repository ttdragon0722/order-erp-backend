using Microsoft.AspNetCore.Mvc;

using erp_server.Services.Repositories;
using erp_server.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace erp_server.Controllers
{
    [ApiController]
    [Route("api")]
    public class MaterialController(MaterialService materialService) : ControllerBase
    {
        private readonly MaterialService _materialService = materialService;

        [Authorize]
        [HttpGet("getMaterials")]
        public async Task<IActionResult> GetMaterials()
        {
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
