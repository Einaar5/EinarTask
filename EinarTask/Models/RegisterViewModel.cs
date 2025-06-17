using System.ComponentModel.DataAnnotations;

namespace EinarTask.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}