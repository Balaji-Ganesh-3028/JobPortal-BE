using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobPortal.Controllers
{
    public class CityController : Controller
    {
        public readonly IConfiguration _configuration;
        public CityController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("citylist")]
        public async Task<IActionResult> StateList(int countryId, int stateId)
        {
            if (countryId == null || stateId == null)
            {
                return StatusCode(401, "Country id and State id are required");
            }

            
            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                var city = new List<MastersList>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("getCity", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CountryId", countryId);
                        command.Parameters.AddWithValue("@StateId", stateId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                // You can process the data here if needed
                                var data = new MastersList
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("id")),
                                    code = reader["code"].ToString(),
                                    value = reader["value"].ToString()
                                };

                                city.Add(data);
                            }
                        }
                    }

                    return Ok(city);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
