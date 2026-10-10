using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace VitaPoint.Server.Tests
{  
    public class AccountRegistrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
    
        private readonly HttpClient _client;

        public AccountRegistrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_WithMismatchedActivationCode_Return404NotFound()
        {
            var invalidRegisterDto = new
            {
                Email = "marcuswright@vitapoint.com",
                Password = "Password123!",
                DOB = "1990-11-23",
                Zip = "80202",
                ActivationCode = "ACT-1234"
            };

            var response = await _client.PostAsJsonAsync("/api/account/register", invalidRegisterDto);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            Assert.NotNull(content);
            Assert.Contains("No eligible patient record found matching those credentials.", content["message"]);
        }
    }
}
