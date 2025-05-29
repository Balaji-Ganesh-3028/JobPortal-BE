using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobPortal.Controllers
{
    public class StateController : Controller
    {
        public readonly IConfiguration _configuration;
        public StateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("stateslist")]
        public async Task<IActionResult> StateList(int countryId)
        {
            if(countryId == null)
            {
                return StatusCode(401, "Country id is required");
            }
            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                var state = new List<MastersList>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("getState", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CountryId", countryId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                // You can process the data here if needed
                                var data = new MastersList
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("id")),
                                    code = reader["code"].ToString(),
                                    name = reader["value"].ToString()
                                };

                                state.Add(data);
                            }
                        }
                    }

                    return Ok(state);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

            return Ok(countryId);
        }
    }
}
