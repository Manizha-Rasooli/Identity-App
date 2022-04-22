using IdentityApp.Models;
using IdentityApp.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : BaseController
    {

        public AdminController(UserManager<AppUser> _userManager,RoleManager<AppRole> _roleManager):base(_userManager,null,_roleManager)
        {
        }
        public IActionResult Index()
        {
            return View(userManager.Users.ToList());
        }

        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }

        public IActionResult Users()
        {
            return View(userManager.Users.ToList());
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRole(RoleViewModel roleViewModel)
        {
            AppRole role = new AppRole();
            role.Name = roleViewModel.Name;
            IdentityResult result = roleManager.CreateAsync(role).Result;
            if(result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                AddModelError(result);
            }
            return View(roleViewModel);
        }
        public IActionResult Roles()
        {
            return View(roleManager.Roles.ToList());
        }

        public IActionResult RoleDelete(string id)
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;
            if(role!=null)
            {
                IdentityResult result = roleManager.DeleteAsync(role).Result;
              
            }

            return RedirectToAction("Roles");
        }
        public IActionResult RoleUpdate(string id)
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                return View(role.Adapt<RoleViewModel>());

            }
            return RedirectToAction("Roles");

        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            AppRole role = roleManager.FindByIdAsync(roleViewModel.Id).Result;
            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = roleManager.UpdateAsync(role).Result;
                if(result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }

            }
            else
            {
                ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu.");
            }
            return RedirectToAction("Roles");

        }

        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            AppUser user = userManager.FindByIdAsync(id).Result;
            ViewBag.userName = user.UserName;
            IQueryable<AppRole> roles = roleManager.Roles;
            List<string> userRoles = userManager.GetRolesAsync(user).Result as List<string>;

            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();
            foreach (var role in roles)
            {
                RoleAssignViewModel roleAssign = new RoleAssignViewModel();
                roleAssign.RoleId = role.Id;
                roleAssign.RoleName = role.Name;
                if (userRoles.Contains(role.Name))
                {
                    roleAssign.Exist = true;
                }
                else
                {
                    roleAssign.Exist = false;
                }
                roleAssignViewModels.Add(roleAssign);
            }
            return View(roleAssignViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            AppUser user = userManager.FindByIdAsync(TempData["userId"].ToString()).Result;

            foreach (var item in roleAssignViewModels)
            {
                if(item.Exist)
                {
                    await userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                   await userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }
            return RedirectToAction("Users");
        }

    }
}
