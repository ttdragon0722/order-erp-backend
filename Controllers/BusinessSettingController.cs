using Microsoft.AspNetCore.Mvc;
using erp_server.Services.Repositories;
using erp_server.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace erp_server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/settings")]
    public class BusinessSettingsController : ControllerBase
    {
        private readonly BusinessSettingsService _businessService;

        public BusinessSettingsController(BusinessSettingsService businessService)
        {
            _businessService = businessService;
        }

        /// <summary>
        /// 取得所有商業設置狀態
        /// </summary>
        /// <returns>商業設置資料</returns>
        [HttpGet("getStatus")]
        public async Task<IActionResult> GetStatus()
        {
            var allSetting = await _businessService.GetAllSettingsAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "取得設定檔",
                Data = allSetting
            });
        }

        /// <summary>
        /// 設定開機狀態
        /// </summary>
        /// <param name="isEnabled">是否啟用開機狀態</param>
        /// <returns>回傳更新結果</returns>
        [HttpPost("setEnableOrderingStatus")]
        public async Task<IActionResult> SetEnableOrderingStatus([FromBody] bool isEnabled)
        {
            await _businessService.SetEnableOrderingStatusAsync(isEnabled);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "成功更新開機狀態",
                Data = isEnabled
            });
        }

        /// <summary>
        /// 設定內用狀態
        /// </summary>
        /// <param name="isEnabled">是否啟用內用狀態</param>
        /// <returns>回傳更新結果</returns>
        [HttpPost("setDineInStatus")]
        public async Task<IActionResult> SetDineInStatus([FromBody] bool isEnabled)
        {
            await _businessService.SetDineInStatusAsync(isEnabled);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "成功更新內用狀態",
                Data = isEnabled
            });
        }

        /// <summary>
        /// 設定外帶狀態
        /// </summary>
        /// <param name="isEnabled">是否啟用外帶狀態</param>
        /// <returns>回傳更新結果</returns>
        [HttpPost("setTakeoutStatus")]
        public async Task<IActionResult> SetTakeoutStatus([FromBody] bool isEnabled)
        {
            await _businessService.SetTakeoutStatusAsync(isEnabled);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "成功更新外帶狀態",
                Data = isEnabled
            });
        }

        /// <summary>
        /// 設定外送狀態
        /// </summary>
        /// <param name="isEnabled">是否啟用外送狀態</param>
        /// <returns>回傳更新結果</returns>

        [HttpPost("setDeliveryStatus")]
        public async Task<IActionResult> SetDeliveryStatus([FromBody] bool isEnabled)
        {
            await _businessService.SetDeliveryStatusAsync(isEnabled);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "成功更新外送狀態",
                Data = isEnabled
            });
        }
    }
}
