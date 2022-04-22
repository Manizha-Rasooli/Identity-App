﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.CustomValidation
{
    public class CustomIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError(){
              Code ="InvalidUserName",
              Description = $"{userName} geçersizdir."
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code ="DuplicateEmail",
                Description =$"{email} kullanılmaktadır."
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError() { 
            Code="DuplicateUserName",
            Description=$"Bu {userName} kullanılmaktadır."
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = $"şifreniz en az {length} karekter olmalı."
            };
        }
    }
}
