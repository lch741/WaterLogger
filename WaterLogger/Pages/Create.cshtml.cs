using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using WaterLogger.Models;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace WaterLogger.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // 从 appsettings.json 获取连接字符串
            string connectionString = _configuration.GetConnectionString("WaterLoggerConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var sql = "INSERT INTO drinking_water (LogDate, Quantity) VALUES (@date, @quantity)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@date", DrinkingWater.Date);
                    command.Parameters.AddWithValue("@quantity", DrinkingWater.Quantity);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
