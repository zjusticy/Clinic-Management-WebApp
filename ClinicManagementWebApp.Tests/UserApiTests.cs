using System.Text;
using System.Text.Json;
using ClinicManagementWebApp.Server.Infrastructure;
using ClinicManagementWebApp.Tests.helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagementWebApp.Tests
{
    public class UserApiTests(WebApplicationFactory<Program> factory) :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient CreateClientWithAuth(string userLevel)
        {
            return factory.WithWebHostBuilder(builder =>

                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(TestAuthHandler.AuthScheme)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            TestAuthHandler.AuthScheme,
                            options => { },
                            serviceProvider => new TestAuthHandler(
                                serviceProvider.GetRequiredService<IOptionsMonitor<AuthenticationSchemeOptions>>(),
                                serviceProvider.GetRequiredService<ILoggerFactory>(),
                                serviceProvider.GetRequiredService<UrlEncoder>(),
                                serviceProvider.GetRequiredService<ISystemClock>(),
                                userLevel
                            )
                        );
                });
        }).CreateClient();
    }


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

    [Fact]
    public async Task Post_Register_EndPointsReturnSuccess()
    {
        /* using (var scope = factory.Services.CreateScope())
         {
             var scopedServices = scope.ServiceProvider;
             var db = scopedServices.GetRequiredService<AppDbContext>();

             *//*Utilities.ReinitializeDbForTests(db);*//*

             await Utilities.SeedTestUser(scopedServices);


         }*/
        var client = factory.CreateClient();

        var request = new
        {
            FirstName = "HHII",
            LastName = "sting",
            Email = "test246@test.com",
            PhoneNumber = "234",
            Gender = 1,
            Password = "1234Qwe[",
            DateOfBirth = "2024-05-10"
        };

        var jsonRequest = JsonSerializer.Serialize(request);

        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("/api/users/register", content);

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetUsers_ReturnsSuccessForAdmin()
    {
        // Arrange
        var client = CreateClientWithAuth("admin");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(TestAuthHandler.AuthScheme);

        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        response.EnsureSuccessStatusCode();
        var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserBriefDTO>>();
        Assert.NotNull(users);
    }
}
}