namespace ClinicManagementWebApp.Server.Features.Appointment.DTOs
{
    public class AppointmentBriefDTO
    {
        /// <summary>
        /// The unique identifier for appointments
        /// </summary>
        public int Id { get; set; }

        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }

        public DateTime Date { get; set; }

        public byte AppointmentStatus { get; set; }

        public string? ReasonForVisit { get; set; }
    }
}
