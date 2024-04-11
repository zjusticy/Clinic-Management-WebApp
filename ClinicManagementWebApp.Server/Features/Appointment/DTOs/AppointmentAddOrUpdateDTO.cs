using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentAddOrUpdateDTO
    {
        public Guid? DoctorId { get; set; }

        public Guid? PatientId { get; set; }

        [DisplayName("Appointment date")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime? Date { get; set; }

        public byte AppointmentStatus { get; set; }

        [DisplayName("Reason for visit")]
        [StringLength(50, ErrorMessage = "The length of the {0} must be less than {1}")]
        public string? ReasonForVisit { get; set; }
    }
}
