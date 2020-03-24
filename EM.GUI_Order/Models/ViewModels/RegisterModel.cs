using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using EM.Domain;
using EM.Domain.Utility;

namespace EM.GUI_Order.Models.ViewModels
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord bevestigen")]
        [Compare("Password", ErrorMessage = "Wachtwoord komt niet overeen met de bevestiging")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = Constants.PhoneRequired)]
        [Display(Name = "Telefoonnummer")]
        [RegularExpression("(\\+31|31|)(-| |)(06|6)(-| |)[0-9]{8}",
            ErrorMessage = Constants.PhoneInvalid)]
        public String PhoneNumber { get; set; }

        public string ReturnUrl { get; set; } = "/";

        [Required(ErrorMessage = Constants.NameRequired)]
        public string CustomerName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }
}
