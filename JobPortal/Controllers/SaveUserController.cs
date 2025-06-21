using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Globalization;

namespace JobPortal.Controllers
{
    public class SaveUserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SaveUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("InsertUser")]
        public async Task<IActionResult> InserOrUpdateUser([FromBody] UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return BadRequest("User details are not available.");
            }

            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("InsertDemographicInformation", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                        command.Parameters.AddWithValue("@LastName", userProfile.LastName);
                        command.Parameters.AddWithValue("@Email", userProfile.Email);
                        command.Parameters.AddWithValue("@Salutation", userProfile.Salutation);
                        command.Parameters.AddWithValue("@Gender", userProfile.Gender);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();
                            if (reader.Read()) 
                            {
                                int userId = Convert.ToInt32(reader["Id"]);
                                string operationType = reader["OperationType"].ToString();
                                string message = operationType == "Inserted" ? "Record Inserted Successfully" : "Record Updated Successfully";

                                reader.Close(); // Close the reader before executing the next command

                                // INSERT INTERESTS 
                                if (userProfile.Interests != null && userProfile.Interests.Any())
                                {
                                    DataTable interestTable = new DataTable();
                                    interestTable.Columns.Add("userid", typeof(int));
                                    interestTable.Columns.Add("intrest_id", typeof(string));

                                    foreach (var interest in userProfile.Interests)
                                    {
                                        interestTable.Rows.Add(userId, interest.value);
                                    }

                                    using (SqlCommand interestCmd = new SqlCommand("InsertUsersInterest", connection))
                                    {
                                        interestCmd.CommandType = CommandType.StoredProcedure;
                                        SqlParameter interestParam = interestCmd.Parameters.AddWithValue("@UserInterests", interestTable);
                                        interestParam.SqlDbType = SqlDbType.Structured;
                                        await interestCmd.ExecuteNonQueryAsync();
                                    }
                                } // INSERT INTERESTS

                                // INSERT EDUCATION DETAILS
                                if (userProfile.EducationInformation == null)
                                {
                                    return BadRequest("Education information is not available.");
                                }
                                else
                                {
                                    DataTable educationInfoTable = new DataTable();

                                    // ADD COLUMN NAMES
                                    educationInfoTable.Columns.Add("UserId", typeof(int));
                                    educationInfoTable.Columns.Add("EducationId", typeof(int));
                                    educationInfoTable.Columns.Add("CredentialInfo", typeof(string));
                                    educationInfoTable.Columns.Add("DegreeCertificate", typeof(string));
                                    educationInfoTable.Columns.Add("Institution", typeof(string));
                                    educationInfoTable.Columns.Add("City", typeof(string));
                                    educationInfoTable.Columns.Add("StateOrProvince", typeof(string));

                                    // ADD DATA
                                    foreach (var education in userProfile.EducationInformation)
                                    {
                                        educationInfoTable.Rows.Add(userId, education.EducationId, education.Credential, education.DegreeCertificate, education.Institution, education.EducationCity, education.EducationState);
                                    }

                                    using (SqlCommand educationCmd = new SqlCommand("InsertEducationInformation", connection))
                                    {
                                        educationCmd.CommandType = CommandType.StoredProcedure;
                                        SqlParameter educationInfoParam = educationCmd.Parameters.AddWithValue("@EducationInfos", educationInfoTable);
                                        educationInfoParam.SqlDbType = SqlDbType.Structured;
                                        await educationCmd.ExecuteNonQueryAsync();
                                    }
                                }
                                // INSERT EDUCATION DETAILS

                                //INSERT EXPERIENCE DETAILS
                                if (userProfile.ExperienceInformation == null)
                                {
                                    return BadRequest("Experience information is not available.");
                                }
                                else
                                { 
                                    DataTable experienceInfoTable = new DataTable();

                                    // ADD COLUMN NAMES
                                    experienceInfoTable.Columns.Add("UserId", typeof(int));
                                    experienceInfoTable.Columns.Add("ExperienceId", typeof(int));
                                    experienceInfoTable.Columns.Add("Employer", typeof(string));
                                    experienceInfoTable.Columns.Add("DOJ", typeof(DateTime));
                                    experienceInfoTable.Columns.Add("JobRole", typeof(string));
                                    experienceInfoTable.Columns.Add("DurationInMonth", typeof(int));

                                    // ADD DATA
                                    foreach (var experience in userProfile.ExperienceInformation)
                                    {                   
                                        DateTime doj = experience.DOJ.ToDateTime(TimeOnly.MinValue);

                                        experienceInfoTable.Rows.Add(userId, experience.ExperienceId, experience.Employer, doj, experience.Role,  experience.DurationInMonth);
                                    }

                                    using (SqlCommand experienceCmd = new SqlCommand("InsertExperienceInformation", connection))
                                    {
                                        experienceCmd.CommandType = CommandType.StoredProcedure;
                                        SqlParameter experienceInfoParam = experienceCmd.Parameters.AddWithValue("@ExperienceInfos", experienceInfoTable);
                                        experienceInfoParam.SqlDbType = SqlDbType.Structured;
                                        await experienceCmd.ExecuteNonQueryAsync();
                                    }
                                }
                                //INSERT EXPERIENCE DETAILS

                                //INSERT ADDRESS DETAILS
                                if(userProfile.Address == null)
                                {
                                    return BadRequest("Address information is not available.");
                                } 
                                else
                                {
                                    DataTable addressInfoTable = new DataTable();

                                    // ADD COLUMN NAMES
                                    addressInfoTable.Columns.Add("UserId", typeof(int));
                                    addressInfoTable.Columns.Add("AddressId", typeof(int));
                                    addressInfoTable.Columns.Add("AddressType", typeof(string));
                                    addressInfoTable.Columns.Add("Country", typeof(string));
                                    addressInfoTable.Columns.Add("StateOrProvince", typeof(string));
                                    addressInfoTable.Columns.Add("City", typeof(string));
                                    addressInfoTable.Columns.Add("DoorNoStreet", typeof(string));
                                    addressInfoTable.Columns.Add("Pincode", typeof(string));

                                    // ADD DATA
                                    foreach (var address in userProfile.Address)
                                    {
                                        addressInfoTable.Rows.Add(userId, address.AddressId, address.Type, address.Country, address.State, address.City, address.DoorNoStreet, address.Pincode);
                                    }

                                    using (SqlCommand addressCmd = new SqlCommand("InsertAddressInformation", connection))
                                    {
                                        addressCmd.CommandType = CommandType.StoredProcedure;
                                        SqlParameter addressInfoParam = addressCmd.Parameters.AddWithValue("@AddressInfos", addressInfoTable);
                                        addressInfoParam.SqlDbType = SqlDbType.Structured;
                                        await addressCmd.ExecuteNonQueryAsync();
                                    }
                                }


                               
                                //INSERT ADDRESS DETAILS

                                await connection.CloseAsync();

                                var data = new
                                {
                                    Message = message,
                                    Id = userId,
                                    OperationType = operationType,
                                };

                                return Ok( data );
                            }
                            else
                            {
                                return BadRequest("No records were inserted or updated.");
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return StatusCode(500, $"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


    }

}
