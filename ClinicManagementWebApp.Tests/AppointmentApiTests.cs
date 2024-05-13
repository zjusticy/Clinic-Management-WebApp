using System.Text;
using System.Text.Json;
using ClinicManagementWebApp.Server.Infrastructure;
using ClinicManagementWebApp.Tests.helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagementWebApp.Tests
{
    public class AppointmentApiTests(WebApplicationFactory<Program> factory) :
        IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task Post_Login_EndPointsReturnSuccess()
        {
            using (var scope = factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();

                /*Utilities.ReinitializeDbForTests(db);*/

                await Utilities.SeedTestUser(scopedServices);


            }
            var client = factory.CreateClient();

            var request = new
            {
                email = "test328@test.com",
                password = "1234Qwe["
            };

            var jsonRequest = JsonSerializer.Serialize(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/users/login", content);

            response.EnsureSuccessStatusCode();
        }
    }
}