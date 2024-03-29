using ClinicManagementWebApp.Server.Features.Appointment.Entity;

namespace ClinicManagementWebApp.Server.Infrastructure.Repositories
{
    public class AppointmentRepository(AppDbContext context) : Repository<AppointmentModel, int>(context), IAppointmentRepository
    {
    }
}
