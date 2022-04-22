using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name = "Eski Şifreniz")]
        [Required(ErrorMessage = "Eski Şifreniz gereklidir!")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Yeni Şifreniz gereklidir!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Onay Yeni Şifreniz")]
        [Required(ErrorMessage = "Onay Yeni Şifreniz gereklidir!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Yeni Şifreniz ve Onay Şifreniz birbirinden farklıdır.")]
        public string ConfirmPassword { get; set; }
    }
}
