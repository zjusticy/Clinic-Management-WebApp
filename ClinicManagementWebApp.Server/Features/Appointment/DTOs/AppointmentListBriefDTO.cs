using ClinicManagementWebApp.Server.Features.Users.DTOs;

namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentListBriefDTO
    {
        /// <summary>
        /// The unique identifier for appointments
        /// </summary>
        public int Id { get; set; }

        public UserIdNameDTO Doctor { get; set; } = new();
        public UserIdNameDTO Patient { get; set; } = new();

        public DateTime Date { get; set; }

        public byte AppointmentStatus { get; set; }

        public string? ReasonForVisit { get; set; }

        public string? Prescription { get; set; }
    }
}
