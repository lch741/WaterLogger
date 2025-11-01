using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.Models;

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
            string connectionString = _configuration.GetConnectionString("WaterLoggerConnection");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sql = "INSERT INTO drinking_water (LogDate, Quantity) VALUES (@date, @quantity)";

                using (var command = new SqliteCommand(sql, connection))
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
