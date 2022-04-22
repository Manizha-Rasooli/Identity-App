using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name = "E-Posta Adresiniz")]
        [Required(ErrorMessage = "E-Posta alanı gereklidir!")]
        [EmailAddress]
        public string Email { get; set; }



        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Şifreniz gereklidir!")]
        [DataType(DataType.Password)]
        public string PasswordNew { get; set; }
    }
}
