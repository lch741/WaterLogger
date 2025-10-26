using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WaterLogger.Models;

namespace WaterLogger.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public DrinkingWaterModel DrinkingWater { get; set; }
        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet(int id)
        {
            DrinkingWater = GetById(id);
            return Page();
        }
        private DrinkingWaterModel GetById(int id)
        {
            String connectionString = _configuration.GetConnectionString("WaterLoggerConnection");
            using (var connection = new SqlConnection(connectionString)){
                connection.Open();
                var sql = "SELECT ID, LogDate, Quantity FROM drinking_water WHERE Id = @id";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(@"id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return new DrinkingWaterModel
                            {
                                Id = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Quantity = reader.GetInt32(2)
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            String connectionString = _configuration.GetConnectionString("WaterLoggerConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM drinking_water WHERE Id = @id";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", DrinkingWater.Id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
