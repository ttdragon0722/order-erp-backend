using Microsoft.AspNetCore.Mvc;
using erp_server.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using erp_server.Models;
using erp_server.Dtos;

namespace erp_server.Controllers
{
    /// <summary>
    /// 負責分類（TypeEntity）相關的 API。
    /// </summary>
    [ApiController]
    [Route("api/classifications")]
    public class ClassificationController(ClassificationService classificationService, ILogger<MaterialController> logger) : ControllerBase
    {
        private readonly ClassificationService _classificationService = classificationService;
        private readonly ILogger<MaterialController> _logger = logger;

        // 取得所有分類
        [HttpGet]
        public async Task<IActionResult> GetAllClassifications()
        {
            var result = await _classificationService.GetAllClassificationsAsync();
            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<ClassificationListDto>>> GetClassificationList()
        {
            var list = await _classificationService.GetClassificationListAsync();
            return Ok(list);
        }

        // 透過 ID 取得分類
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassificationById(Guid id)
        {
            var result = await _classificationService.GetClassificationByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // 新增分類
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddClassification([FromBody] AddClassificationDto dto)
        {
            var result = await _classificationService.AddClassificationAsync(dto.Name, dto.Enable);
            return CreatedAtAction(nameof(GetClassificationById), new { id = result.Id }, result);
        }

        // 修改名稱
        [HttpPatch("{id}/name")]
        [Authorize]
        public async Task<IActionResult> EditClassificationName(Guid id, [FromBody] string name)
        {
            var result = await _classificationService.UpdateClassificationNameAsync(id, name);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // 修改啟用狀態
        [HttpPatch("{id}/enable")]
        [Authorize]
        public async Task<IActionResult> EditClassificationEnable(Guid id, [FromBody] bool enable)
        {
            var result = await _classificationService.UpdateClassificationEnableStatusAsync(id, enable);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("update-sort-order")]
        public async Task<IActionResult> UpdateSortOrder([FromBody] IEnumerable<UpdateSortOrderDto> updatedSortOrders)
        {
            var result = await _classificationService.UpdateSortOrderAsync(updatedSortOrders);
            return Ok(result);
        }


    }
}
