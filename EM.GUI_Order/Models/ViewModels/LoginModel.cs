using System.ComponentModel.DataAnnotations;

namespace EM.GUI_Order.Models.ViewModels
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [UIHint("wachtwoord")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Ingelogd blijven")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
