using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagementWebApp.Server.Features.Users.DTOs
{
    public class PatientAddOrUpdateDTO : UserAddOrUpdateDTO
    {

        [DisplayName("Date of birth")]
        [Required(ErrorMessage = "{0} is required")]
        public DateTime? DateOfBirth { get; set; }

    }
}
