using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace VitaPoint.Server.Tests
{
    public class AccountPasswordTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public AccountPasswordTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact]
        public async Task Register_WeakPassword_Returns409Conflict()
        {
            var weakPasswordPayload = new
            {
                Email = "marcuswright@vitapoint.com",
                Password = "123",
                DOB = "1990-11-23",
                Zip = "80202",
                ActivationCode = "ACT-1002"
            };

            var response = await _client.PostAsJsonAsync("/api/account/register", weakPasswordPayload);
            string responseBody = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

            _output.WriteLine($"--> HTTP Status Code: {(int)response.StatusCode} {response.StatusCode}");
            _output.WriteLine($"--> Response Body: {responseBody}");

            var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            Assert.NotNull(content);
            Assert.True(content.ContainsKey("message") || content.ContainsKey("errors"));

        }
    }
}
