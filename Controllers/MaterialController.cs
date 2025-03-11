using Microsoft.AspNetCore.Mvc;
using erp_server.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace erp_server.Controllers
{
    /// <summary>
    /// 負責處理物料（Material）相關的 API。
    /// </summary>
    [ApiController]
    [Route("api")]
    public class MaterialController(MaterialService materialService, ILogger<MaterialController> logger) : ControllerBase
    {
        private readonly MaterialService _materialService = materialService;
        private readonly ILogger<MaterialController> _logger = logger;

        /// <summary>
        /// 取得所有物料資料
        /// </summary>
        /// <returns>成功時回傳 200 OK，失敗時回傳 500</returns>
        /// <response code="200">成功取得物料資料</response>
        /// <response code="500">伺服器內部錯誤</response>
        [Authorize]
        [HttpGet("getMaterials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaterials()
        {
            try
            {
                var materialData = await _materialService.GetAll();
                return Ok(materialData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得物料時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError, "伺服器內部錯誤，請稍後再試");
            }
        }
    }
}
