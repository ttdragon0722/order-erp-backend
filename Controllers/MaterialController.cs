using Microsoft.AspNetCore.Mvc;

using erp_server.Services.Repositories;
using erp_server.Dtos;

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
