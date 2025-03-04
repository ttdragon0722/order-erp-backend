# BaseService API Documentation

## Overview
`BaseService<T>` 提供通用的資料庫存取功能，支援同步與非同步操作。

---

## Class: `BaseService<T>`
### Properties:
- `sync` - 提供同步操作的 `SyncService`
- `async` - 提供非同步操作的 `AsyncService`

---

## Class: `SyncService`
### Methods:
- `void Insert(T entity)` - 插入一筆資料。
- `List<T> GetAll()` - 取得所有資料。
- `T GetById(int id)` - 根據 ID 取得資料。
- `void Update(T entity)` - 更新資料。
- `void Delete(T entity)` - 刪除資料。

---

## Class: `AsyncService`
### Methods:
- `Task InsertAsync(T entity)` - 非同步插入一筆資料。
- `Task<List<T>> GetAllAsync()` - 非同步取得所有資料。
- `Task<T> GetByIdAsync(int id)` - 非同步根據 ID 取得資料。
- `Task UpdateAsync(T entity)` - 非同步更新資料。
- `Task DeleteAsync(T entity)` - 非同步刪除資料。

---

## Usage Example
### 繼承 `BaseService<T>` 創建特定資料表的服務
```csharp
public class MaterialService : BaseService<Material>
{
    public MaterialService(AppDbContext context) : base(context) { }
}
```

### 使用 `MaterialService`
```csharp
var materialService = new MaterialService(context);
materialService.sync.Insert(new Material { Name = "Steel" });
var materials = await materialService.async.GetAllAsync();
```

