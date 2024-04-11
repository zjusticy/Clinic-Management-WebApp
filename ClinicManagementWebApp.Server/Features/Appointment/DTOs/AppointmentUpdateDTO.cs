using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentUpdateDTO : AppointmentAddOrUpdateDTO
    {
        public int Id { get; set; }

        [DisplayName("Prescription")]
        [StringLength(300, ErrorMessage = "The length of the {0} must be less than {1}")]
        public string? Prescription { get; set; }
    }
}
