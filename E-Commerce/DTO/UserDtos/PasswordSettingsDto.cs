using System.ComponentModel.DataAnnotations;

namespace TestToken.DTO.UserDtos
{
    public class PasswordSettingsDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword),ErrorMessage ="New Password and Confirmation Passwoed don't match.!!")]
        public string ConfirmNewPassword { get; set; }

    }
}
