using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;

namespace JobPortal.Controllers
{
    public class UpdateUserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UpdateUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfile request)
        {
            if (request == null || request.UserId == 0 )
            {
                return BadRequest("Invalid user update request.");
            }

            // Here you would typically call a service to update the user in the database.
            // For this example, we will just return a success message.

            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                // Create a new SqlConnection using the connection string from configuration
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection asynchronously
                    await connection.OpenAsync();

                    // Create a SqlCommand to update the user profile

                    // Interest information is handled separately in a stored procedure
                    using(SqlCommand interestCmd = new SqlCommand("UpdateUserInterest", connection))
                    {
                        interestCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        if(request.Interests != null && request.Interests.Count > 0)
                        {
                            DataTable interestTable = new DataTable();
                            // Define the structure of the DataTable to match the stored procedure parameters
                            interestTable.Columns.Add("UserId", typeof(int));
                            interestTable.Columns.Add("Interest_Id", typeof(string));
                            interestTable.Columns.Add("Interest_Value", typeof(string));

                            foreach (var interest in request.Interests)
                            {
                                // Add each interest to the DataTable
                                Console.WriteLine(interest.value);
                                interestTable.Rows.Add(request.UserId, interest.interestId, interest.value);
                            }

                            interestCmd.CommandType = CommandType.StoredProcedure;
                            SqlParameter interestParam = interestCmd.Parameters.AddWithValue("@UpdateUserInterests", interestTable);
                            interestParam.SqlDbType = SqlDbType.Structured;
                            await interestCmd.ExecuteNonQueryAsync();
                        }

                    }; // Close the interest command

                    // Education information is handled separately in a stored procedure
                    using(SqlCommand educationCmd = new SqlCommand("UpdateEducationInformation", connection))
                    {
                        educationCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        if(request.EducationInformation != null && request.EducationInformation.Count > 0)
                        {
                            DataTable educationTable = new DataTable();
                            // Define the structure of the DataTable to match the stored procedure parameters
                            educationTable.Columns.Add("EducationId", typeof(int));
                            educationTable.Columns.Add("UserId", typeof(int));
                            educationTable.Columns.Add("CredentialInfo", typeof(string));
                            educationTable.Columns.Add("DegreeCertificate", typeof(string));
                            educationTable.Columns.Add("Institution", typeof(string));
                            educationTable.Columns.Add("City", typeof(string));
                            educationTable.Columns.Add("StateOrProvince", typeof(string));

                            foreach (var education in request.EducationInformation)
                            {
                                // Add each education record to the DataTable
                                educationTable.Rows.Add(education.EducationId ,request.UserId, education.Credential, education.DegreeCertificate, education.Institution, education.EducationCity, education.EducationState);
                            }

                            SqlParameter educationParam = educationCmd.Parameters.AddWithValue("@UpdateEducationInfo", educationTable);
                            educationParam.SqlDbType = SqlDbType.Structured;
                            await educationCmd.ExecuteNonQueryAsync();
                        }
                    }; // Close the education command

                    // Experience information is handled separately in a stored procedure
                    using(SqlCommand experienceCmd = new SqlCommand("UpdateExperienceInformation", connection))
                    {
                        experienceCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        if(request.ExperienceInformation != null && request.ExperienceInformation.Count > 0)
                        {
                            DataTable experienceTable = new DataTable();
                            // Define the structure of the DataTable to match the stored procedure parameters
                            experienceTable.Columns.Add("ExperienceId", typeof(int));
                            experienceTable.Columns.Add("UserId", typeof(int));
                            experienceTable.Columns.Add("Employer", typeof(string));
                            experienceTable.Columns.Add("DOJ", typeof(DateTime));
                            experienceTable.Columns.Add("JobRole", typeof(string));
                            experienceTable.Columns.Add("DurationInMonth", typeof(int));

                            foreach (var experience in request.ExperienceInformation)
                            {
                                // DateTime doj = experience.DOJ.ToDateTime(TimeOnly.MinValue);
                                DateTime doj = experience.DOJ.ToDateTime(TimeOnly.MinValue);
                                // Add each experience record to the DataTable
                                experienceTable.Rows.Add(experience.ExperienceId, request.UserId, experience.Employer, doj, experience.Role, experience.DurationInMonth);
                            }

                            SqlParameter experienceParam = experienceCmd.Parameters.AddWithValue("@UpdateExperienceInfo", experienceTable);
                            experienceParam.SqlDbType = SqlDbType.Structured;
                            await experienceCmd.ExecuteNonQueryAsync();
                        }
                    }; // Close the experience command

                    // Address information is handled separately in a stored procedure
                    using(SqlCommand addressCmd = new SqlCommand("UpdateAddressInformation", connection))
                    {
                        addressCmd.CommandType = System.Data.CommandType.StoredProcedure;

                        if(request.Address != null && request.Address.Count > 0)
                        {
                            DataTable addressTable = new DataTable();
                            // Define the structure of the DataTable to match the stored procedure parameters
                            addressTable.Columns.Add("UserId", typeof(int));
                            addressTable.Columns.Add("AddressId", typeof(int));
                            addressTable.Columns.Add("AddressType", typeof(string));
                            addressTable.Columns.Add("Country", typeof(string));
                            addressTable.Columns.Add("StateOrProvince", typeof(string));
                            addressTable.Columns.Add("City", typeof(string));
                            addressTable.Columns.Add("StrDoorNoStreeteet", typeof(string));
                            addressTable.Columns.Add("Pincode", typeof(string));

                            foreach (var addr in request.Address)
                            {
                                // Add each address record to the DataTable
                                addressTable.Rows.Add(request.UserId, addr.AddressId, addr.Type, addr.Country, addr.State, addr.City, addr.DoorNoStreet, addr.Pincode);
                            }

                            SqlParameter addressParam = addressCmd.Parameters.AddWithValue("@UpdateAddressInfos", addressTable);
                            addressParam.SqlDbType = SqlDbType.Structured;
                            await addressCmd.ExecuteNonQueryAsync();
                        }
                    }; // Close the address command

                    return Ok(new { Message = "User updated successfully." });
                };
            } 
            catch (SqlException sqlEx)
            {
                // Log the SQL exception (not implemented in this example)
                return StatusCode(500, "Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented in this example)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }

        }
    }
}
