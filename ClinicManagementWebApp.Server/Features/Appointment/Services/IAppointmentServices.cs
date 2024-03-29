using ClinicManagementWebApp.Server.Features.Appointment.Entity;

namespace ClinicManagementWebApp.Server.Features.Appointment.Services
{
    public interface IAppointmentServices
    {
        Task<ICollection<AppointmentModel>?> GetAppointmentListForPatientByUserIdAsync(Guid userId);

        Task<ICollection<AppointmentModel>?> GetAppointmentListForDoctorByUserIdAsync(Guid userId);
    }
}
