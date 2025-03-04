using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using erp_server.Data;

namespace erp_server.Services
{
    /// <summary>
    /// 基礎服務類別，提供通用的資料庫存取操作（CRUD），統一使用非同步方法。
    /// </summary>
    /// <typeparam name="T">對應的資料表模型類別</typeparam>
    public class BaseService<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseService(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// 插入新資料（非同步）
        /// </summary>
        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 取得所有資料（非同步）
        /// </summary>
        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// 根據 ID 取得單筆資料（非同步）
        /// </summary>
        public async Task<T?> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// 更新資料（非同步）
        /// </summary>
        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 刪除資料（非同步）
        /// </summary>
        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 根據指定欄位名稱與值查詢單筆資料（非同步）
        /// </summary>
        public async Task<T?> GetByField(string fieldName, object value)
        {
            var property = typeof(T).GetProperty(fieldName);
            if (property == null)
            {
                throw new ArgumentException($"類別 {typeof(T).Name} 沒有欄位 {fieldName}");
            }

            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<object>(e, fieldName).Equals(value));
        }
    }
}