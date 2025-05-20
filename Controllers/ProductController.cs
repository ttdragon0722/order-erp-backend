using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using erp_server.Services.Repositories;
using erp_server.Dtos;
using System.Threading.Tasks;
using erp_server.Models;


namespace erp_server.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController(
        ILogger<ProductController> logger,
        ProductService productService
        ) : ControllerBase
    {
        private readonly ProductService _productService = productService;
        private readonly ILogger<ProductController> _logger = logger;

        [Authorize]
        [HttpPost("create")]
        public async Task<ActionResult<Product>> Create([FromBody] CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("產品名稱不可為空");

            try
            {
                var product = await _productService.CreateAsync(dto.Name, dto.Price, dto.TypeId, dto.Enable);
                return Ok(product);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<ProductListDto>>> GetProductList()
        {
            try
            {
                var result = await _productService.GetProductList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得產品清單時發生錯誤");
                return StatusCode(500, "伺服器錯誤，請稍後再試");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCart>> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductCartByIdAsync(id);
                if (product == null)
                {
                    return NotFound("找不到該產品");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得產品資料時發生錯誤");
                return StatusCode(500, "伺服器錯誤，請稍後再試");
            }
        }

    }
}
