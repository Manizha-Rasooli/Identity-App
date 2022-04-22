using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.CustomValidation
{
    public class CustomUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();
            string[] digits = new string[] {"1","2","3","4","5","6","7","8","9","0"};

            foreach (var digit in digits)
            {
                if(user.UserName[0].ToString()==digit)
                {
                    errors.Add(new IdentityError() {
                    Code="UserNamesFirstCharacterCantBenDigit",
                    Description="kullanıcı adının ilk harfı sayı olamaz!"
                    });
                }
            }

            if(errors.Count == 0)
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
