using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementWebApp.Server.Features.User.Services
{
    public class UserServices(UserManager<UserModel> userManager) : IUserServices
    {
        public async Task<ICollection<AppointmentModel>?> GetAppointmentListForUserByUserIdAsync(Guid userId, int status, string role)
        {
            var users = userManager.Users;


            if (role == "patient")
            {
                var user = await users.Include(u => u.AppointmentsForPatient)
                .ThenInclude(ap => ap.Doctor).FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) { return null; }

                return [.. user.AppointmentsForPatient.Where(obj => obj.AppointmentStatus == status)];
            }

            if (role == "doctor")
            {
                var user = await users.Include(u => u.AppointmentsForDoc)
                 .ThenInclude(ap => ap.Patient).FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null) { return null; }

                return [.. user.AppointmentsForDoc.Where(obj => obj.AppointmentStatus == status)];
            }


            return [];
        }

    }
}
