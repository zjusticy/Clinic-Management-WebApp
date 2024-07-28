using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ClinicManagementWebApp.Server.Features.Users.DTOs;
using ClinicManagementWebApp.Server.Infrastructure;
using ClinicManagementWebApp.Tests.helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace ClinicManagementWebApp.Tests
{
    public class UserApiTests(WebApplicationFactory<Program> factory, ITestOutputHelper output) :
        IClassFixture<WebApplicationFactory<Program>>
    {
        private async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            var client = factory.CreateClient();

            var request = new
            {
                email = "admin@admin.com",
                password = "P@ssw0rd"
            };

            var jsonRequest = JsonSerializer.Serialize(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/users/login", content);

            response.EnsureSuccessStatusCode();

            return client;
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

        /* [Fact]
         public async Task Post_Register_EndPointsReturnSuccess()
         {
             *//* using (var scope = factory.Services.CreateScope())
              {
                  var scopedServices = scope.ServiceProvider;
                  var db = scopedServices.GetRequiredService<AppDbContext>();

                  *//*Utilities.ReinitializeDbForTests(db);*//*

                  await Utilities.SeedTestUser(scopedServices);


              }*//*
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
         }*/

        [Fact]
        public async Task GetUsers_ReturnsSuccessForAdmin()
        {
            // Arrange
            var client = await GetAuthenticatedClientAsync();
            // Act
            var response = await client.GetAsync("/api/users/");

            string jsonString = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserBriefDTO>>();
            Assert.NotNull(users);
        }
    }
}