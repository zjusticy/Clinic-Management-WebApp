using System.ComponentModel.DataAnnotations;

namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentAddOrUpdateDTO
    {

        public Guid DoctorId { get; set; }

        public Guid PatientId { get; set; }

        public DateTime Date { get; set; }

        public byte AppointmentStatus { get; set; }

        [MaxLength(50)]
        public string? ReasonForVisit { get; set; }
    }
}
