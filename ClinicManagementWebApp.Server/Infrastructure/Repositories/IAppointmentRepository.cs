using ClinicManagementWebApp.Server.Features.Appointment.Entity;

namespace ClinicManagementWebApp.Server.Infrastructure.Repositories
{
    public interface IAppointmentRepository : IRepository<AppointmentModel, int>
    {
        Task<IEnumerable<AppointmentModel>> GetByStatusAsync(int status);
    }
}
