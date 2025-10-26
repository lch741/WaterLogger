using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== SQL Server 连接诊断工具 ===\n");

        // 测试多种连接字符串
        string[] connectionStrings = new[]
        {
            // 方式1: 使用命名管道（Named Pipes）- 不需要 TCP/IP
            "Server=np:\\\\.\\pipe\\MSSQL$SQLEXPRESS\\sql\\query;Database=WaterLogger;Trusted_Connection=True;TrustServerCertificate=True;",
            
            // 方式2: 使用 (local)\SQLEXPRESS
            "Server=(local)\\SQLEXPRESS;Database=WaterLogger;Trusted_Connection=True;TrustServerCertificate=True;",
            
            // 方式3: 使用 localhost\SQLEXPRESS
            "Server=localhost\\SQLEXPRESS;Database=WaterLogger;Trusted_Connection=True;TrustServerCertificate=True;",
            
            // 方式4: 使用计算机名\SQLEXPRESS
            $"Server={Environment.MachineName}\\SQLEXPRESS;Database=WaterLogger;Trusted_Connection=True;TrustServerCertificate=True;",
            
            // 方式5: 原始方式 127.0.0.1\SQLEXPRESS
            "Server=127.0.0.1\\SQLEXPRESS;Database=WaterLogger;Trusted_Connection=True;TrustServerCertificate=True;",
            
            // 方式6: 使用 . 表示本地\SQLEXPRESS
            "Server=.\\SQLEXPRESS;Database=WaterLogger;Trusted_Connection=True;TrustServerCertificate=True;",
        };

        string[] descriptions = new[]
        {
            "命名管道方式（推荐，不需要TCP/IP）",
            "(local)\\SQLEXPRESS",
            "localhost\\SQLEXPRESS",
            $"{Environment.MachineName}\\SQLEXPRESS",
            "127.0.0.1\\SQLEXPRESS",
            ".\\SQLEXPRESS"
        };

        bool anySuccess = false;

        for (int i = 0; i < connectionStrings.Length; i++)
        {
            Console.WriteLine($"[{i + 1}/{connectionStrings.Length}] 测试: {descriptions[i]}");

            if (TryConnect(connectionStrings[i]))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ 成功！使用这个连接字符串:\n{connectionStrings[i]}\n");
                Console.ResetColor();
                anySuccess = true;
                break; // 找到一个可用的就停止
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ 失败\n");
                Console.ResetColor();
            }
        }

        if (!anySuccess)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n⚠️  所有连接方式都失败了！");
            Console.WriteLine("\n请检查:");
            Console.WriteLine("1. SQL Server 服务是否启动（services.msc 查看 SQL Server (SQLEXPRESS)）");
            Console.WriteLine("2. 实例名称是否确实是 SQLEXPRESS");
            Console.WriteLine("3. WaterLogger 数据库是否已创建");
            Console.WriteLine("4. 当前 Windows 用户是否有访问权限");
            Console.ResetColor();
        }

        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }

    static bool TryConnect(string connectionString)
    {
        try
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            // 执行简单查询确认连接有效
            using var cmd = new SqlCommand("SELECT 1", conn);
            cmd.ExecuteScalar();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}