using erp_server.Dtos;
using erp_server.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace erp_server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TypeMaterialsController(TypeMaterialsService typeMaterialsService, ILogger<TypeMaterialsController> logger) : ControllerBase
    {

        private readonly TypeMaterialsService _typeMaterialsService = typeMaterialsService;
        private readonly ILogger<TypeMaterialsController> _logger = logger;


        [HttpPost("bind")]
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

    }
}