using Microsoft.EntityFrameworkCore;
using DiaryApp.Models; // 引入你的 Model 命名空間
using DotNetEnv; // 只有本地開發需要
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// 讀取 .env
Env.Load("../.env");

// 加入資料庫服務
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (string.IsNullOrEmpty(connectionString))
    {
        // 沒設定環境變數 -> 本地開發用 SQLite
        options.UseSqlite("Data Source=diary.db");
    }
    else if (connectionString.StartsWith("postgresql://"))
    {
        // Render 的 URL 需要特殊處理

        var databaseUri = new Uri(connectionString);

        var userInfo = databaseUri.UserInfo.Split(':', 2); // 使用 Split(':', 2) 確保只分成兩段

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port == -1 ? 5432 : databaseUri.Port, // 沒填 Port 預設 5432
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.AbsolutePath.TrimStart('/'),
            SslMode = SslMode.Require,
            TrustServerCertificate = true,
        };

        options.UseNpgsql(builder.ConnectionString);
    }
    else
    {
        // 本地 PostgreSQL 測試時直接用
        options.UseNpgsql(connectionString);
    }
});


// 加入 API Controller 支援
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// 🔥 新增：啟動時自動建表
// using (var scope = app.Services.CreateScope())
// {
//     var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     dbContext.Database.Migrate();
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 顯示詳細錯誤頁面
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// 建立 API Controller
app.MapControllers();

app.Run();
