using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace VitaPoint.Server.Tests
{
    public class MessageCreateTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        private string JohnDoeUserId = "d864c718-a2d8-4961-a40b-58caebeeb0f3";
        private string AuthorizedDoctorId = "b8325d06-05a5-44de-9a30-8e09133b21aa";
        private string UnauthorizedDoctorId = "c978a84c-6536-4fa7-bf2d-e844ff11432c";
        

        public MessageCreateTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
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
        public async Task CreateUnauthorizedMessage_Returns403Forbidden()
        {
            await HandleLogin("johndoe@vitapoint.com", "Admin123!");

            var unauthorizedMessagePayload = new
            {
                UserIds = new string[] { JohnDoeUserId, UnauthorizedDoctorId },
                Subject = "Unauthorized message request",
                Content = "Attempting to bypass frontend contact list"
            };

            var response = await _client.PostAsJsonAsync("/api/message", unauthorizedMessagePayload);
            string responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine("\n\n--------Unauthorized Message------------------");
            _output.WriteLine($"--> HTTP Status Code: {(int)response.StatusCode} {response.StatusCode}");
            _output.WriteLine($"--> Response Body: {responseBody}");
            _output.WriteLine("--------Unauthorized Message------------------\n\n");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task SendAuthorizedMessage_Returns201Created()
        {
            await HandleLogin("johndoe@vitapoint.com", "Admin123!");

            var unauthorizedMessagePayload = new
            {
                UserIds = new string[] { JohnDoeUserId, AuthorizedDoctorId },
                Subject = "Follow up question regarding lab results",
                Content = "Hello Doctor, I had a question about my recent test results."
            };

            var response = await _client.PostAsJsonAsync("/api/message", unauthorizedMessagePayload);
            string responseBody = await response.Content.ReadAsStringAsync();

            _output.WriteLine("\n\n--------Authorized Message------------------");
            _output.WriteLine($"--> HTTP Status Code: {(int)response.StatusCode} {response.StatusCode}");
            _output.WriteLine($"--> Response Body: {responseBody}");
            _output.WriteLine("--------Authorized Message------------------\n\n");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
