using ClinicManagementWebApp.Server.Features.Appointment.Entity;

namespace ClinicManagementWebApp.Server.Features.User.Services
{
    public interface IUserServices
    {
        Task<ICollection<AppointmentModel>?> GetAppointmentListForUserByUserIdAsync(Guid userId, int status, string role);
    }
}
