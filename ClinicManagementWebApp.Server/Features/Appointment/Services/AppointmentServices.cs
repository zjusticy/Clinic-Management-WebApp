using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementWebApp.Server.Features.Appointment.Services
{
    public class AppointmentServices(UserManager<UserModel> userManager) : IAppointmentServices
    {
        public async Task<ICollection<AppointmentModel>?> GetAppointmentListForPatientByUserIdAsync(Guid userId)
        {
            var user = await userManager.Users.Include(u => u.AppointmentsForPatient)
                .ThenInclude(ap => ap.Doctor).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) { return null; }

            return [.. user.AppointmentsForPatient];
        }

        public async Task<ICollection<AppointmentModel>?> GetAppointmentListForDoctorByUserIdAsync(Guid userId)
        {
            var user = await userManager.Users.Include(u => u.AppointmentsForDoc)
                .ThenInclude(ap => ap.Patient).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) { return null; }

            return [.. user.AppointmentsForDoc];
        }
    }
}
