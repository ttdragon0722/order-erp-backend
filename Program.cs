using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;
using Microsoft.EntityFrameworkCore;
using erp_server.Data;

// repository import
using erp_server.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ 保留原本的 API 設定
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ✅ 連上database server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    options.UseMySql(connectionString, serverVersion);
});

builder.Services.AddScoped<UserService>();

// 🔍 測試資料庫連線 (加在這裡)
try
{
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // 嘗試開啟連線
    db.Database.OpenConnection();
    Console.WriteLine("✅ 資料庫連線成功！");

    // 關閉連線
    db.Database.CloseConnection();
}
catch (Exception ex)
{
    Console.WriteLine("❌ 無法連線到資料庫！");
    Console.WriteLine($"🔍 錯誤訊息: {ex.Message}");
}



// ✅ 加入 YARP 反向代理設定
builder.Services.AddReverseProxy()
    .LoadFromMemory(new[]
    {
        new RouteConfig
        {
            RouteId = "nextjs",
            ClusterId = "nextjs_cluster",
            Match = new RouteMatch { Path = "{**catch-all}" } // 捕捉所有前端路由
        }
    },
    new[]
    {
        new ClusterConfig
        {
            ClusterId = "nextjs_cluster",
            Destinations = new Dictionary<string, DestinationConfig>
            {
                { "nextjs", new DestinationConfig { Address = "http://localhost:3000" } }
            }
        }
    });

var app = builder.Build();

// ✅ 保留原本的 API 相關設定
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers(); // 保留 API 路由

// ✅ 加入反向代理
app.UseRouting();
app.MapReverseProxy(); // 轉發 Next.js 的請求

app.Run();
