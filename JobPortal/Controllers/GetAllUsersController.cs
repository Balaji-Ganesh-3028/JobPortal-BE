using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobPortal.Controllers
{
    public class GetAllUsersController : Controller
    {
        public readonly IConfiguration _configuration;
        public GetAllUsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                // Here you would typically fetch users from the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("GetAllEmployees", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            var users = new List<UserProfile>(); // Replace object with your user model
                            List<InterestList> interests = new List<InterestList>();
                            List<EducationInformation> educationInformation = new List<EducationInformation>();
                            List<ExperienceInformation> experienceInformation = new List<ExperienceInformation>();
                            List<Address> address = new List<Address>();


                            while (await reader.ReadAsync())
                            {
                                // Safely get Interests as string (handles nulls)
                                string interestsJson = reader["Interests"] == DBNull.Value ? string.Empty : reader["Interests"]?.ToString();
                                string educationJson = reader["EducationInformation"] == DBNull.Value ? string.Empty : reader["EducationInformation"]?.ToString();
                                string experienceJson = reader["ExperienceInformation"] == DBNull.Value ? string.Empty : reader["ExperienceInformation"]?.ToString();
                                string addressJson = reader["AddressInformation"] == DBNull.Value ? string.Empty : reader["AddressInformation"]?.ToString();

                                if (!string.IsNullOrWhiteSpace(interestsJson))
                                {
                                    var interestsData = JsonConvert.DeserializeObject<List<InterestDto>>(interestsJson) ?? new List<InterestDto>();

                                    interests = interestsData.Select(i => new InterestList
                                    {
                                        interestId = i.InterestId,
                                        value = i.value
                                    }).ToList();
                                }

                                if (!string.IsNullOrWhiteSpace(educationJson))
                                {
                                    var eduInfoData = JsonConvert.DeserializeObject<List<EducationInformationDto>>(educationJson) ?? new List<EducationInformationDto>();

                                    educationInformation = eduInfoData.Select(e => new EducationInformation
                                    {
                                        EducationId = e.Id,
                                        DegreeCertificate = e.DegreeCertificate,
                                        Institution = e.Institution,
                                        Credential = e.CredentialInfo,
                                        EducationState = e.StateOrProvince,
                                        EducationCity = e.City
                                    }).ToList();
                                }

                                if (!string.IsNullOrWhiteSpace(experienceJson))
                                {
                                    var expInfoData = JsonConvert.DeserializeObject<List<ExperienceInformationDto>>(experienceJson) ?? new List<ExperienceInformationDto>();

                                    experienceInformation = expInfoData.Select(e => new ExperienceInformation
                                    {
                                        ExperienceId = e.Id,
                                        Employer = e.Employer,
                                        Role = e.JobRole,
                                        DOJ = e.DOJ,
                                        DurationInMonth = e.DurationInMonth,
                                    }).ToList();
                                }

                                if (!string.IsNullOrWhiteSpace(addressJson))
                                {
                                    var addrInfoData = JsonConvert.DeserializeObject<List<AddressDto>>(addressJson) ?? new List<AddressDto>();

                                    address = addrInfoData.Select(a => new Address
                                    {
                                        AddressId = a.Id,
                                        Type = a.AddressType,
                                        DoorNoStreet = a.DoorNoStreet,
                                        Pincode = a.Pincode,
                                        Country = a.Country,
                                        State = a.StateOrProvince,
                                        City = a.City,
                                    }).ToList();
                                }
                               

                                // Map the data to your user model
                                var user = new UserProfile
                                {
                                    UserId = Convert.ToInt32(reader["Id"]),
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Salutation = reader["Salutation"].ToString(),
                                    Gender = reader["Gender"].ToString(),
                                    Interests = interests,
                                    EducationInformation = educationInformation,
                                    ExperienceInformation = experienceInformation,
                                    Address = address
                                };
                                users.Add(user);
                            }


                            return Ok(users);
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
