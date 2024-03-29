using System.ComponentModel.DataAnnotations;
using ClinicManagementWebApp.Server.Features.Appointment.Entity;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementWebApp.Server.Features.Users.Entity
{
    public class UserModel : IdentityUser<Guid>
    {

        [MaxLength(50)]
        public string? FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? LastName { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public byte? Gender { get; set; }

        public byte? Status { get; set; }

        public ICollection<AppointmentModel> AppointmentsForDoc { get; } = [];

        public ICollection<AppointmentModel> AppointmentsForPatient { get; } = [];
    }
}
