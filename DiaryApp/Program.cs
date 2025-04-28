using Microsoft.EntityFrameworkCore;
using DiaryApp.Models; // 引入你的 Model 命名空間

var builder = WebApplication.CreateBuilder(args);

// 加入資料庫服務
//builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlite("Data Source=diary.db")); // 使用本地 SQLite 資料庫
// 使用 PostgreSQL 資料庫
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("DATABASE_URL 環境變數沒有設定！");
    }

    options.UseNpgsql(connectionString);
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
