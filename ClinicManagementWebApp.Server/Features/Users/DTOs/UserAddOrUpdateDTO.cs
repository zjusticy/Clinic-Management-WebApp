using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementWebApp.Server.Features.Users.DTOs
{
    public class UserAddOrUpdateDTO
    {
        [DisplayName("First name")]
        [StringLength(50, ErrorMessage = "The length of the {0} must be less than {1}")]
        [Required(ErrorMessage = "{0} is required")]
        public string FirstName { get; set; } = string.Empty;

        [DisplayName("First name")]
        [StringLength(50, ErrorMessage = "The length of the {0} must be less than {1}")]
        [Required(ErrorMessage = "{0} is required")]
        public string LastName { get; set; } = string.Empty;

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(254, MinimumLength = 3, ErrorMessage = "The length of the {0} must be between {2} and {1}")]


        /// <summary>
        /// The user email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        [DisplayName("Cellphone  Number")]
        [StringLength(15, ErrorMessage = "The length of the {0} must be less than {1}")]
        public string PhoneNumber { get; set; } = string.Empty;

        /*        public DateTime DateOfBirth { get; set; }*/


        [DisplayName("Gender")]
        [Required(ErrorMessage = "{0} is required")]
        public byte Gender { get; set; }

        public byte? Status { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} is required")]
        [MinLength(8, ErrorMessage = "The length of the {0} must be at least {1}")]
        [PasswordWithLetterAndNumber]
        public string Password { get; set; } = string.Empty;
    }

    public class PasswordWithLetterAndNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var password = value as string;
            if (password == null) { return false; }

            return password.Any(char.IsLetter) && password.Any(char.IsDigit);
        }

        public override string FormatErrorMessage(string name)
        {
            return "The " + name + " must contain at least one letter and one number";
        }

    }
}
