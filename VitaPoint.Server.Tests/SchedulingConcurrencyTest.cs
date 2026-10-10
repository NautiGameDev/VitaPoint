using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace VitaPoint.Server.Tests
{
    public class SchedulingConcurrencyTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ITestOutputHelper _output;

        public SchedulingConcurrencyTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        private HttpClient CreateAuthenticatedClient()
        {
            return _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });
        }

        private async Task HandleLogin(HttpClient client, string email, string password)
        {
            var loginPayload = new { Email = email, Password = password };
            var response = await client.PostAsJsonAsync("/api/account/login", loginPayload);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _output.WriteLine($"--> Login failed for [{email}] with status {response.StatusCode}: {errorContent}");
            }

            response.EnsureSuccessStatusCode();            
        }

        [Fact]
        public async Task BookAPpointment_ConcerrencyRequest_OnlyOneSucceeds()
        {
            var clientA = CreateAuthenticatedClient();
            var clientB = CreateAuthenticatedClient();

            await HandleLogin(clientA, "sarahconnor@vitapoint.com", "Admin123!");
            await HandleLogin(clientB, "johndoe@vitapoint.com", "Admin123!");

            string doctorId = "61278cc4-c352-4587-a739-1d85afbeaf31";
            string aptDate = DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd");
            string aptTime = "10:00:00";

            var appointmentPayload = new
            {
                DoctorId = doctorId,
                Date = aptDate,
                TimeSlot = aptTime,
                Category = "Sick Visit",
                Notes = "High fever"
            };

            var taskA = clientA.PostAsJsonAsync("/api/appointment", appointmentPayload);
            var taskB = clientB.PostAsJsonAsync("/api/appointment", appointmentPayload);

            var responses = await Task.WhenAll(taskA, taskB);

            var responseA = responses[0];
            var responseB = responses[1];

            string bodyA = await responseA.Content.ReadAsStringAsync();
            string bodyB = await responseB.Content.ReadAsStringAsync();

            _output.WriteLine("\n\n--------Scheduling Concurrency Test Results------------");
            _output.WriteLine($"--> Client A Status: {(int)responseA.StatusCode} {responseA.StatusCode} | Body: {bodyA}");
            _output.WriteLine($"--> Client B Status: {(int)responseB.StatusCode} {responseB.StatusCode} | Body: {bodyB}");
            _output.WriteLine("-------------------------------------------------------\n\n");

            bool oneSucceeded = (responseA.StatusCode == HttpStatusCode.Created || responseA.StatusCode == HttpStatusCode.OK) ^
                                (responseB.StatusCode == HttpStatusCode.Created || responseB.StatusCode == HttpStatusCode.OK);

            bool oneWasRejected = responseA.StatusCode == HttpStatusCode.Conflict || responseB.StatusCode == HttpStatusCode.Conflict ||
                                 responseA.StatusCode == HttpStatusCode.InternalServerError || responseB.StatusCode == HttpStatusCode.InternalServerError;

            Assert.True(oneSucceeded, "Expected exactly one booking attempt to succeed.");
            Assert.True(oneWasRejected, "Expected the second booking attempt to be rejected due to a schedule conflict.");
        }
    }
}
