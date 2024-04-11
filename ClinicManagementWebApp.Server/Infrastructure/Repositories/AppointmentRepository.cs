using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementWebApp.Server.Infrastructure.Repositories
{
    public class AppointmentRepository(AppDbContext context) : Repository<AppointmentModel, int>(context), IAppointmentRepository
    {
        private readonly DbSet<AppointmentModel> _entities = context.Set<AppointmentModel>();
        public async Task<IEnumerable<AppointmentModel>> GetByStatusAsync(int status)
        {
            return await _entities.Include(ap => ap.Doctor).Include(ap => ap.Patient).Where(obj => obj.AppointmentStatus == status).ToListAsync();
        }
    }
}
