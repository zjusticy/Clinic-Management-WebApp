using Microsoft.AspNetCore.Identity;

namespace ClinicManagementWebApp.Server.Features.Users.DTOs
{
    public class UserBriefDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public byte Gender { get; set; }

        public ICollection<IdentityRole> Roles { get; set; } = [];
    }
}
