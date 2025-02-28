using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

// ✅ 保留原本的 API 設定
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
