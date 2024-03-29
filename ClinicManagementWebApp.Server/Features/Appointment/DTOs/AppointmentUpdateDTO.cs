using System.ComponentModel.DataAnnotations;

namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentUpdateDTO : AppointmentAddOrUpdateDTO
    {
        public int Id { get; set; }

        [MaxLength(300)]
        public string? Prescription { get; set; }
    }
}
