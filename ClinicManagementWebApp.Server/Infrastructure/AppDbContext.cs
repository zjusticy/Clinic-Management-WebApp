using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementWebApp.Server.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<UserModel, IdentityRole<Guid>, Guid>(options)
    {

        public DbSet<AppointmentModel> Appointments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppointmentModel>()
                .HasOne(ap => ap.Patient).WithMany(p => p.AppointmentsForPatient)
                .HasForeignKey(ap => ap.PatientId).IsRequired().OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AppointmentModel>()
                .HasOne(ap => ap.Doctor).WithMany(d => d.AppointmentsForDoc)
                .HasForeignKey(ap => ap.DoctorId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
