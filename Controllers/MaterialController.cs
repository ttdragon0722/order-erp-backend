using Microsoft.AspNetCore.Mvc;
using erp_server.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using erp_server.Dtos;
using System.Threading.Tasks;
using erp_server.Models.Enums;

namespace erp_server.Controllers
{
    /// <summary>
    /// 負責處理物料（Material）相關的 API。
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/materials")]
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaterials()
        {
            try
            {
                var materialData = await _materialService.GetAllMaterialsWithTagsAsync();
                return Ok(materialData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得物料時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError, "伺服器內部錯誤，請稍後再試");
            }
        }

        [HttpPost]
        public IActionResult AddMaterial([FromBody] string name)
        {
            var material = materialService.AddMaterial(name);
            return Ok(material);
        }

        [HttpPost("with-tags")]
        public async Task<IActionResult> AddMaterialWithTags([FromBody] AddMaterialWithTagsDto dto)
        {
            var material = await materialService.AddMaterialWithTags(dto.Name, dto.TagIds);
            return Ok(material);
        }
        
        [HttpPut("update-stock")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateStockRequest request)
        {
            if (!Enum.IsDefined(typeof(StockStatus), request.StockStatus))
            {
                return BadRequest("無效的庫存狀態");
            }

            var success = await _materialService.UpdateStockAsync(request.Id, request.StockStatus, request.StockAmount);

            if (!success)
            {
                return BadRequest("更新失敗，可能是資料不存在或庫存數量錯誤");
            }

            return Ok("庫存更新成功");
        }

        [HttpPut("update-name")]
        public IActionResult UpdateName([FromQuery] Guid id, [FromBody] string newName)
        {
            if (!materialService.UpdateMaterialName(id, newName))
                return NotFound("Material not found");
            return Ok();
        }

        [HttpPut("update-enable")]
        public IActionResult UpdateEnable([FromQuery] Guid id, [FromBody] bool enable)
        {
            if (!materialService.UpdateMaterialEnable(id, enable))
                return NotFound("Material not found");
            return Ok();
        }

        [HttpDelete("drop/{id}")]
        public IActionResult DropMaterial(Guid id)
        {
            try
            {
                var result = materialService.Drop(id);
                if (!result) return NotFound(new { message = "找不到資料" });
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // 回傳詳細錯誤
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
            }
        }

    }
}
