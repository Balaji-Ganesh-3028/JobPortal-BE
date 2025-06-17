using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class GetAllUsersController : Controller
    {
        public readonly IConfiguration _configuration;
        public GetAllUsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
