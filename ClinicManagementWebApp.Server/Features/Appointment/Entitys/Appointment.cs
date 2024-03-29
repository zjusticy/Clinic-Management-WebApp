using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using ClinicManagementWebApp.Server.Infrastructure;

namespace ClinicManagementWebApp.Server.Features.Appointment.Entity
{
    public class AppointmentModel : ITableObject<int>
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Doctor))]
        public Guid DoctorId { get; set; }

        public UserModel? Doctor { get; set; }

        [ForeignKey(nameof(Patient))]
        public Guid PatientId { get; set; }

        public UserModel? Patient { get; set; }

        public DateTime Date { get; set; }

        public byte AppointmentStatus { get; set; }

        [MaxLength(50)]
        public string? ReasonForVisit { get; set; }

        [MaxLength(300)]
        public string? Prescription { get; set; }
    }
}
