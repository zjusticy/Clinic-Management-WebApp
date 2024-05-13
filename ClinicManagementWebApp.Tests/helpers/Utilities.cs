using System.Security.Claims;
using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using ClinicManagementWebApp.Server.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicManagementWebApp.Tests.helpers
{
    public static class Utilities
    {
        // <snippet1>
        public static void InitializeDbForTests(AppDbContext db)
        {
            db.Appointments.AddRange(GetSeedingMessages());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(AppDbContext db)
        {
            db.Appointments.RemoveRange(db.Appointments);
            InitializeDbForTests(db);
        }

        private static List<AppointmentModel> GetSeedingMessages()
        {
            return new List<AppointmentModel>()
        {
            new AppointmentModel(){
               /* Id = 0,*/
   DoctorId =  Guid.Parse("eb9e5427-afdf-45af-8ad8-08dc4eb4773d"),
        Date = DateTime.Parse("2024-03-28T20:43:56.112Z"),
  AppointmentStatus = 0,
  ReasonForVisit = "laugh more",
  PatientId = Guid.Parse("13779069-3173-41a6-8ad7-08dc4eb4773d"),

 },
            new AppointmentModel(){
                /*Id = 1,*/
   DoctorId =  Guid.Parse("eb9e5427-afdf-45af-8ad8-08dc4eb4773d"),
        Date = DateTime.Parse("2024-03-29T20:43:56.112Z"),
  AppointmentStatus = 0,
  ReasonForVisit = "Cry more",
  PatientId = Guid.Parse("13779069-3173-41a6-8ad7-08dc4eb4773d" ),
            },
            new AppointmentModel(){ 
                /*Id = 2,*/
   DoctorId =  Guid.Parse("eb9e5427-afdf-45af-8ad8-08dc4eb4773d"),
        Date = DateTime.Parse("2024-03-30T20:43:56.112Z"),
  AppointmentStatus = 0,
  ReasonForVisit = "Think more",
  PatientId = Guid.Parse("13779069-3173-41a6-8ad7-08dc4eb4773d")}
        };
        }

        public static async Task SeedTestUser(IServiceProvider sp)
        {
            var userManager = sp.GetRequiredService<UserManager<UserModel>>();
            var userStore = sp.GetRequiredService<IUserStore<UserModel>>();
            var emailStore = (IUserEmailStore<UserModel>)userStore;
            UserModel admin = new();

            if (!userManager.Users.Any(x => x.Email == "test328@test.com"))
            {
                await userStore.SetUserNameAsync(admin, "test328@test.com", CancellationToken.None);
                await emailStore.SetEmailAsync(admin, "test328@test.com", CancellationToken.None);

                var result = await userManager.CreateAsync(admin, "1234Qwe[");
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                await userManager.AddClaimAsync(admin, new Claim("level", "admin"));

            }
        }


        // </snippet1>
    }
}
