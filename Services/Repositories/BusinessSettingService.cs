using System.Threading.Tasks;
using erp_server.Data;
using erp_server.Models;
using Microsoft.EntityFrameworkCore;

namespace erp_server.Services.Repositories
{
    /// <summary>
    /// 提供對 BusinessSettings 表格的操作，包括取得和設定開機、內用、外帶、外送等狀態。
    /// </summary>
    public class BusinessSettingsService(AppDbContext context) : BaseService<BusinessSettings>(context)
    {
        /// <summary>
        /// 取得所有的商業設置。
        /// 如果資料庫中沒有任何設定，則返回一個新的 BusinessSettings 物件。
        /// </summary>
        /// <returns>返回 BusinessSettings 物件。</returns>
        public async Task<BusinessSettings> GetAllSettingsAsync()
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            return settings ?? new BusinessSettings(); // 如果沒有找到資料，返回一個新的 BusinessSettings 物件
        }

        /// <summary>
        /// 取得開機狀態（點餐系統啟用狀態）。
        /// </summary>
        /// <returns>返回一個布林值，表示開機狀態。</returns>
        public async Task<bool> GetEnableOrderingStatusAsync()
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            return settings?.EnableOrdering ?? false;
        }

        /// <summary>
        /// 設定開機狀態（點餐系統啟用狀態）。
        /// </summary>
        /// <param name="isEnabled">布林值，表示是否啟用開機狀態。</param>
        public async Task SetEnableOrderingStatusAsync(bool isEnabled)
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();

            if (settings != null)
            {
                settings.EnableOrdering = isEnabled;
                await Update(settings);
            }
        }

        /// <summary>
        /// 取得內用狀態（是否啟用內用功能）。
        /// </summary>
        /// <returns>返回一個布林值，表示內用狀態。</returns>
        public async Task<bool> GetDineInStatusAsync()
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            return settings?.EnableDineIn ?? false;
        }

        /// <summary>
        /// 取得外帶狀態（是否啟用外帶功能）。
        /// </summary>
        /// <returns>返回一個布林值，表示外帶狀態。</returns>
        public async Task<bool> GetTakeoutStatusAsync()
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            return settings?.EnableTakeout ?? false;
        }

        /// <summary>
        /// 取得外送狀態（是否啟用外送功能）。
        /// </summary>
        /// <returns>返回一個布林值，表示外送狀態。</returns>
        public async Task<bool> GetDeliveryStatusAsync()
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            return settings?.EnableDelivery ?? false;
        }

        /// <summary>
        /// 設定內用狀態（啟用或禁用內用功能）。
        /// </summary>
        /// <param name="isEnabled">布林值，表示是否啟用內用狀態。</param>
        public async Task SetDineInStatusAsync(bool isEnabled)
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            if (settings != null)
            {
                settings.EnableDineIn = isEnabled;
                await Update(settings);

            }
        }

        /// <summary>
        /// 設定外送狀態（啟用或禁用外送功能）。
        /// </summary>
        /// <param name="isEnabled">布林值，表示是否啟用外送狀態。</param>
        public async Task SetDeliveryStatusAsync(bool isEnabled)
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            if (settings != null)
            {
                settings.EnableDelivery = isEnabled;

                await Update(settings);
            }
        }

        /// <summary>
        /// 設定外帶狀態（啟用或禁用外帶功能）。
        /// </summary>
        /// <param name="isEnabled">布林值，表示是否啟用外帶狀態。</param>
        public async Task SetTakeoutStatusAsync(bool isEnabled)
        {
            var settings = await _context.BusinessSettings.SingleOrDefaultAsync();
            if (settings != null)
            {
                settings.EnableTakeout = isEnabled;
                await Update(settings);
            }
        }
    }
}
