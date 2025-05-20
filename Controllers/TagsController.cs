using Microsoft.AspNetCore.Mvc;
using erp_server.Services.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using erp_server.Models;
using erp_server.Dtos;
using erp_server.Models.Enums;

namespace erp_server.Controllers
{
    /// <summary>
    /// 負責分類（TypeEntity）相關的 API。
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/tag")]
    public class TagController(TagService tagService, ILogger<TagController> logger) : ControllerBase
    {
        private readonly TagService _tagService = tagService;
        private readonly ILogger<TagController> _logger = logger;

        /// <summary>
        /// 新增 Tag。
        /// </summary>
        /// <param name="name">Tag 的名稱。</param>
        /// <param name="color">Tag 的顏色。</param>
        /// <returns>返回新增的 Tag 物件。</returns>
        [HttpPost]
        public async Task<ActionResult<Tag>> CreateTag(string name, ColorName color)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest("Tag 名稱不可為空");
                }

                // 呼叫 TagService 來新增 Tag
                var createdTag = await _tagService.AddTagAsync(name, color);

                // 回傳新增的 Tag 物件並且 HTTP 201 Created
                return CreatedAtAction(nameof(CreateTag), new { id = createdTag.Id }, createdTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增 Tag 發生錯誤");
                return StatusCode(500, "伺服器內部錯誤");
            }
        }


        /// <summary>
        /// 獲取所有的 Tag。
        /// </summary>
        /// <returns>返回所有的 Tag 物件。</returns>
        [HttpGet]
        public async Task<ActionResult<List<Tag>>> GetTags()
        {
            try
            {
                // 呼叫 TagService 來獲取所有 Tag
                var tags = await _tagService.GetAllTagsAsync();

                // 如果沒有 Tag，回傳 NoContent
                if (tags == null || tags.Count == 0)
                {
                    return NoContent();
                }

                return Ok(tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取 Tag 發生錯誤");
                return StatusCode(500, "伺服器內部錯誤");
            }
        }
    }
}
