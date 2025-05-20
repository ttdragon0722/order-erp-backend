using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using erp_server.Services.Repositories;
using erp_server.Dtos;
using System.Threading.Tasks;


namespace erp_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/bind")]
    public class BindController(
        ILogger<BindController> logger, 
        TypeMaterialsService typeMaterialsService,
        MaterialTagService materialTagService
        ) : ControllerBase
    {
        private readonly TypeMaterialsService _typeMaterialsService = typeMaterialsService;
        private readonly MaterialTagService _materialTagService = materialTagService;

        private readonly ILogger<BindController> _logger = logger;

        [HttpPost("materials2type")]
        public IActionResult BindMaterialsToType([FromBody] TypeMaterialMappingDto request)
        {
            try
            {
                if (request.TypeMaterials == null || request.TypeMaterials.Count == 0)
                {
                    return BadRequest("TypeMaterials 不能為空");
                }

                bool result = _typeMaterialsService.AddMappings(request.TypeEntityId, request.TypeMaterials);
                if (result)
                {
                    return Ok(new { message = "Type-Material 綁定成功" });
                }
                else
                {
                    return BadRequest("綁定失敗，請稍後再試");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "綁定 type-material 發生錯誤");
                return StatusCode(500, "發生錯誤，請稍後再試");
            }
        }

        [HttpPost("tag2Materials")]
        public async Task<IActionResult> BindTagToMaterials([FromBody] TagMaterialMappingDto request)
        {
            try
            {
                if (request.Tags == null || request.Tags.Count == 0)
                {
                    return BadRequest("Tags 不能為空");
                }

                bool result = await _materialTagService.AddMappings(request.MaterialId, request.Tags);
                if (result)
                {
                    return Ok(new { message = "Type-Material 綁定成功" });
                }
                else
                {
                    return BadRequest("綁定失敗，請稍後再試");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "綁定 type-material 發生錯誤");
                return StatusCode(500, "發生錯誤，請稍後再試");
            }
        }
    }
}
