using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using erp_server.Services.Repositories;
using erp_server.Dtos;

namespace erp_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/option")]
    public class OptionController(ILogger<OptionController> logger, OptionService optionService) : ControllerBase
    {

        private readonly OptionService _optionService = optionService;
        private readonly ILogger<OptionController> _logger = logger;
        [HttpPost("create")]
        public async Task<IActionResult> CreateOption([FromBody] CreateOptionRequest request)
        {
            try
            {
                var result = await _optionService.CreateOption(
                    request.Name,
                    request.Price,
                    request.Type,
                    request.Children,
                    request.Require ?? false,
                    request.OptionDepends // ✅ 加上這行
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateOption 發生錯誤");
                return StatusCode(500, "內部錯誤，請稍後再試");
            }
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<OptionResponse>>> GetAllOptions() {
            try {
                var result = await _optionService.GetAllOptionsAsync();
                return Ok(result);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "get all 發生錯誤");
                return StatusCode(500, "內部錯誤，請稍後再試");
            }
        }
        
        
        [HttpDelete("drop/{id}")]
        public IActionResult DropById(Guid id) {
            try {
                var result = _optionService.Drop(id);
                if (!result) return NotFound(new { message = "找不到資料" });
                return Ok(new { success = true});
            }
            catch (Exception ex) {
                _logger.LogError(ex, "get all 發生錯誤");
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });

            }
        }

    }
}
