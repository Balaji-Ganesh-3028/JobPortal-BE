using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobPortal.Controllers
{
    public class DeleteUserController : Controller
    {
        private readonly IConfiguration _configuration;
        public DeleteUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpDelete("deleteUser/{userId}")]
        public async Task<IActionResult> deleteUser(int userId)
        {
            if(userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("deleteUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected == 0)
                        {
                            return NotFound("User not found.");
                        }

                        await connection.CloseAsync();
                        return Ok(new { Message = "User deleted successfully." });

                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
