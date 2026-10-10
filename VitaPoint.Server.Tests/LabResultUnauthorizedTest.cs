using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace VitaPoint.Server.Tests
{
    public class LabResultUnauthorizedTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public LabResultUnauthorizedTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });
            _output = output;
        }

        private async Task HandleLogin(string email, string password)
        {
            var loginPayload = new { Email = email, Password = password };
            var response = await _client.PostAsJsonAsync("/api/account/login", loginPayload);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _output.WriteLine($"-->Login failed with status {response.StatusCode}: {errorContent}");
            }

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task FetchUnauthorizedLabResult_Returns404NotFound()
        {
            await HandleLogin("sarahconnor@vitapoint.com", "Admin123!");

            var response = await _client.GetAsync("/api/labresult/1");
            string responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine("\n\n--------Unauthorized Lab Result Access------------");
            _output.WriteLine($"--> HTTP Status Code: {(int)response.StatusCode} {response.StatusCode}");
            _output.WriteLine($"--> Response Body: {responseBody}");
            _output.WriteLine("--------Unauthorized Lab Result Access----------------\n\n");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
