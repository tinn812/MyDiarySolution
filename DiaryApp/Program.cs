using Microsoft.EntityFrameworkCore;
using DiaryApp.Models; // 引入你的 Model 命名空間
using DotNetEnv; // 只有本地開發需要

var builder = WebApplication.CreateBuilder(args);

Env.Load();


// 取得資料庫連線字串
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(databaseUrl))
{
    throw new Exception("沒有設定 DATABASE_URL 環境變數。請檢查 Render 設定或 .env 檔。");
}

string connectionString;

if (databaseUrl.StartsWith("postgresql://"))
{
    // ➔ Render 給的是 postgresql://，需要轉換
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}
else
{
    // ➔ 本地自訂 PostgreSQL（或直接用 SQLite）
    connectionString = databaseUrl;
}

// 加入資料庫服務
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (connectionString.Contains("Host="))
    {
        options.UseNpgsql(connectionString); // PostgreSQL
    }
    else
    {
        options.UseSqlite(connectionString); // SQLite
    }
});




// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// 🔥 新增：啟動時自動建表
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

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

app.Run();
