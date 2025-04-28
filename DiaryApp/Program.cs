using Microsoft.EntityFrameworkCore;
using DiaryApp.Models; // 引入你的 Model 命名空間
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
// 讀取 .env
Env.Load();

// 加入資料庫服務
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (string.IsNullOrEmpty(connectionString))
    {
        // 如果沒有設環境變數，則使用 SQLite 本地開發
        options.UseSqlite("Data Source=diary.db");
    }
    else if (connectionString.StartsWith("postgresql://"))
    {
        // Render 的連線字串是 URL 格式，要轉成 Npgsql 格式
        var databaseUri = new Uri(connectionString);
        var userInfo = databaseUri.UserInfo.Split(':');
        var npgsqlConnectionString = $"Host={databaseUri.Host};Port={databaseUri.Port};Username={userInfo[0]};Password={userInfo[1]};Database={databaseUri.AbsolutePath.TrimStart('/')};SSL Mode=Require;Trust Server Certificate=true";

        options.UseNpgsql(npgsqlConnectionString);
    }
    else
    {
        // 本地 PostgreSQL 直接使用
        options.UseNpgsql(connectionString);
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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
