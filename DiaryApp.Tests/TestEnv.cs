using System;
using DotNetEnv;

class TestEnv
{
    static void Test_DatabaseUrl(string[] args)
    {
        // 讀取 .env
        Env.Load();

        // 讀取環境變數
        var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (string.IsNullOrEmpty(dbUrl))
        {
            Console.WriteLine("目前是本地 SQLite 模式");
        }
        else
        {
            Console.WriteLine($"目前使用的資料庫連線字串是：{dbUrl}");
        }
    }
}
