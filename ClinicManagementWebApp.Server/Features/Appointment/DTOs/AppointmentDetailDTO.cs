using ClinicManagementWebApp.Server.Features.Users.DTOs;

namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentDetailDTO
    {
        /// <summary>
        /// The unique identifier for appointments
        /// </summary>
        public int AppointmenId { get; set; }

        public required UserBriefDTO Doctor { get; set; }

        public required UserBriefDTO Patient { get; set; }

        public DateTime Date { get; set; }

        public byte AppointmentStatus { get; set; }

        public string? ReasonForVisit { get; set; }

        public string? Prescription { get; set; }
    }
}
