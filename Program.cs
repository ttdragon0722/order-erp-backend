using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;
using Microsoft.EntityFrameworkCore;
using erp_server.Data;

// repository import
using erp_server.Services.Repositories;

// todo 刪除這個模組
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using erp_server.Services;


var builder = WebApplication.CreateBuilder(args);

// 設定資料庫連線
ConfigureDatabase(builder);

// 設定服務
ConfigureServices(builder);

// 設定反向代理
ConfigureReverseProxy(builder);

// 創建應用程序
var app = builder.Build();

// 設定應用的中介軟體
ConfigureMiddleware(app);

// 開始執行
app.Run();

void ConfigureDatabase(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        var serverVersion = ServerVersion.AutoDetect(connectionString);
        options.UseMySql(connectionString, serverVersion);
    });
}
// 將 TestDatabaseConnection 移動到 Configure 方法中
void TestDatabaseConnection(WebApplication app)
{
    try
    {
        using var scope = app.Services.CreateScope(); // 使用 CreateScope，避免 BuildServiceProvider 警告
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
}
// 封裝 Scoped 服務註冊邏輯
void RegisterScopedServices(IServiceCollection services)
{
    builder.Services.AddScoped<DatabaseTester>();
    services.AddScoped<UserService>();
    services.AddScoped<MaterialService>();

    // 可以在這裡繼續新增其他的 Scoped 服務
}


void ListScopedServices(IServiceCollection services)
{
    var scopedServices = services
        .Where(s => s.Lifetime == ServiceLifetime.Scoped)
        .ToList();

    if (scopedServices.Any())
    {
        Console.WriteLine("已註冊的 Scoped 服務：");
        foreach (var service in scopedServices)
        {
            Console.WriteLine($"- {service.ServiceType.Name}");
        }
    }
    else
    {
        Console.WriteLine("❌ 沒有註冊任何 Scoped 服務");
    }
}

void ConfigureServices(WebApplicationBuilder builder)
{
    try
    {
        // ✅ 設定 API 相關服務
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // ✅ 設定其他服務
        RegisterScopedServices(builder.Services);

        // 列出已註冊的 Scoped 服務
        Console.WriteLine("✅ API服務設定成功！");
        ListScopedServices(builder.Services);
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ 無法設定服務！");
        Console.WriteLine($"🔍 錯誤訊息: {ex.Message}");
    }
}

void ConfigureReverseProxy(WebApplicationBuilder builder)
{
    // ✅ 加入 YARP 反向代理設定
    builder.Services.AddReverseProxy()
        .LoadFromMemory(new[] {
            new RouteConfig
            {
                RouteId = "nextjs",
                ClusterId = "nextjs_cluster",
                Match = new RouteMatch { Path = "{**catch-all}" } // 捕捉所有前端路由
            }
        }, new[] {
            new ClusterConfig
            {
                ClusterId = "nextjs_cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "nextjs", new DestinationConfig { Address = "http://localhost:3000" } }
                }
            }
        });
    Console.WriteLine("✅ YARP反向代理設定成功！");

}

void ConfigureMiddleware(WebApplication app)
{
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
    
    
    // debug: 在此測試資料庫連線
    TestDatabaseConnection(app);
    Console.WriteLine("✅ 應用中介軟體設定完成");

}
