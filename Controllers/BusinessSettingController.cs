using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using erp_server.Services.Repositories;
using System;
using System.Threading.Tasks;

namespace erp_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/settings")]
    public class BusinessSettingsController(ILogger<BusinessSettingsController> logger, BusinessSettingsService businessService) : ControllerBase
    {
        private readonly ILogger<BusinessSettingsController> _logger = logger;
        private readonly BusinessSettingsService _businessService = businessService;

        /// <summary>
        /// 取得所有商業設置狀態。
        /// </summary>
        [HttpGet("getStatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var allSetting = await _businessService.GetAllSettingsAsync();
                return Ok(allSetting);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商業設置時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "伺服器發生錯誤，請稍後再試。",
                    ErrorCode = "INTERNAL_SERVER_ERROR"
                });
            }
        }

        /// <summary>
        /// 設定開機狀態
        /// </summary>
        [HttpPost("setEnableOrderingStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetEnableOrderingStatus([FromBody] bool? isEnabled)
        {
            if (isEnabled == null)
            {
                return BadRequest(new { Message = "請提供有效的 isEnabled 參數。" });
            }

            try
            {
                await _businessService.SetEnableOrderingStatusAsync(isEnabled.Value);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "設定開機狀態時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "伺服器發生錯誤，請稍後再試。",
                    ErrorCode = "INTERNAL_SERVER_ERROR"
                });
            }
        }

        /// <summary>
        /// 設定內用狀態
        /// </summary>
        [HttpPost("setDineInStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetDineInStatus([FromBody] bool? isEnabled)
        {
            if (isEnabled == null)
            {
                return BadRequest(new { Message = "請提供有效的 isEnabled 參數。" });
            }

            try
            {
                await _businessService.SetDineInStatusAsync(isEnabled.Value);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "設定內用狀態時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "伺服器發生錯誤，請稍後再試。",
                    ErrorCode = "INTERNAL_SERVER_ERROR"
                });
            }
        }

        /// <summary>
        /// 設定外帶狀態
        /// </summary>
        [HttpPost("setTakeoutStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetTakeoutStatus([FromBody] bool? isEnabled)
        {
            if (isEnabled == null)
            {
                return BadRequest(new { Message = "請提供有效的 isEnabled 參數。" });
            }

            try
            {
                await _businessService.SetTakeoutStatusAsync(isEnabled.Value);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "設定外帶狀態時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "伺服器發生錯誤，請稍後再試。",
                    ErrorCode = "INTERNAL_SERVER_ERROR"
                });
            }
        }

        /// <summary>
        /// 設定外送狀態
        /// </summary>
        [HttpPost("setDeliveryStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetDeliveryStatus([FromBody] bool? isEnabled)
        {
            if (isEnabled == null)
            {
                return BadRequest(new { Message = "請提供有效的 isEnabled 參數。" });
            }

            try
            {
                await _businessService.SetDeliveryStatusAsync(isEnabled.Value);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "設定外送狀態時發生錯誤");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "伺服器發生錯誤，請稍後再試。",
                    ErrorCode = "INTERNAL_SERVER_ERROR"
                });
            }
        }
    }
}
