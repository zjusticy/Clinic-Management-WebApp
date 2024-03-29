namespace ClinicManagementWebApp.Server.Features.Users.DTOs
{
    public class UserIdNameDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
    }
}
