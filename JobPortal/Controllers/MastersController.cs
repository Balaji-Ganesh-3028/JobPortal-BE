using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JobPortal.Controllers
{
    public class MastersController : Controller
    {
        private readonly IConfiguration _configuration;

        public MastersController(IConfiguration configuration) {
            _configuration = configuration;
        }

        [HttpGet("masterslist")]
        public async Task<IActionResult> MastersList()
        {
            // Simulate some asynchronous operation
            string connectionString = _configuration["ConnectionStrings:JobPortalDB"];

            try
            {
                var salutation = new List<MastersList>();
                var gender = new List<MastersList>();
                var interest = new List<MastersList>();
                var credential = new List<MastersList>();
                var addressType = new List<MastersList>();
                var country = new List<MastersList>();

                // TO GET THE DETAILS FOR GENDER, SALUTATION, CREDENTIAL, ADDRESSTYPE, COUNTRY FROM THE DATABASE
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Fetching data from GenderSalutation table
                    using (SqlCommand SalutationCommand = new SqlCommand("getSalutation", connection))
                    {
                        SalutationCommand.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await SalutationCommand.ExecuteReaderAsync())
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

                                salutation.Add(data);
                            }

                        }
                    }

                    // Fetching data from Gender table
                    using (SqlCommand GenderCommand = new SqlCommand("getGender", connection))
                    {
                        GenderCommand.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await GenderCommand.ExecuteReaderAsync())
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

                                gender.Add(data);
                            }

                        }
                    }

                    // Fetching data from Interest table
                    using (SqlCommand InterestCommand = new SqlCommand("getInterest", connection))
                    {
                        InterestCommand.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await InterestCommand.ExecuteReaderAsync())
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

                                interest.Add(data);
                            }

                        }
                    }

                    // Fetching data from Credential table
                    using (SqlCommand CredentialCommand = new SqlCommand("getCredential", connection))
                    {
                        CredentialCommand.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await CredentialCommand.ExecuteReaderAsync())
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

                                credential.Add(data);
                            }

                        }
                    }

                    // Fetching data from Address type table
                    using (SqlCommand AddressTypeCommand = new SqlCommand("getAddressType", connection))
                    {
                        AddressTypeCommand.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await AddressTypeCommand.ExecuteReaderAsync())
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

                                addressType.Add(data);
                            }

                        }
                    }

                    // Fetching data from Country table
                    using (SqlCommand CountryCommand = new SqlCommand("getCountry", connection))
                    {
                        CountryCommand.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = await CountryCommand.ExecuteReaderAsync())
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

                                country.Add(data);
                            }

                        }
                    }

                    // RETURN ALL THE LIST IN ARRAY
                    return Ok(new {
                        salutation = salutation, 
                        gender = gender,
                        interest = interest,
                        credential = credential,
                        addressType = addressType,
                        country = country
                    });

                }
            } 
            catch
            {
                return StatusCode(500, "Internal server error while fetching masters list.");
            }
        }
    }
}
