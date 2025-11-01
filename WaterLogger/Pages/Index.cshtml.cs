using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.Models;
namespace WaterLogger.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DrinkingWaterModel> Records { get; set; }
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            Records = GetAllRecords();
            ViewData["Total"] = Records.AsEnumerable().Sum(x => x.Quantity);
        }

        public List<DrinkingWaterModel> GetAllRecords()
        {
            string connectionString = _configuration.GetConnectionString("WaterLoggerConnection");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var sql = "SELECT Id, LogDate, Quantity FROM drinking_water ORDER BY LogDate DESC";
                using (var command = new SqliteCommand(sql, connection))
                {
                    var tableData = new List<DrinkingWaterModel>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tableData.Add(new DrinkingWaterModel
                            {
                                Id = reader.GetInt32(0),
                                Date = reader.GetDateTime(1),
                                Quantity = reader.GetInt32(2)
                            });
                        }
                    }
                    return tableData;
                }
            }
        }
    }
}
