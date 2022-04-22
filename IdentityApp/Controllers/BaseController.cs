using IdentityApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<AppUser> userManager { get; }
        protected SignInManager<AppUser> signInManager { get; }
        protected AppUser currentUser=> userManager.FindByNameAsync(User.Identity.Name).Result;

        protected RoleManager<AppRole> roleManager { get; }

        public BaseController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager=null)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
            this.roleManager = _roleManager;
        }

        public void AddModelError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        
    }
}
