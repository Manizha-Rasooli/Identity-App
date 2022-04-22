using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Identity;
using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using IdentityApp.Enums;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;

namespace IdentityApp.Controllers
{
    [Authorize]
    public class MemberController : BaseController
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public MemberController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, IWebHostEnvironment hostEnvironment):base(_userManager,_signInManager)
        {
           
            webHostEnvironment = hostEnvironment;

        }
        public IActionResult Index()
        {
            AppUser user = currentUser; 
            UserViewModel userViewModel = user.Adapt<UserViewModel>(); // it takes only the properties wich contain the UserViewModel Class

            return View(userViewModel);
        }

        public IActionResult UserEdit()
        {
            AppUser user = currentUser;
            UserViewModel userViewModel = user.Adapt<UserViewModel>();
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel,IFormFile userPicture)
        {
            ModelState.Remove("Password");
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            if (ModelState.IsValid)
            {
                AppUser user = currentUser;
                string uniqueFileName = null;

                if(userPicture!=null && userPicture.Length>0)
                {
                   

                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "UserPicture");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + userPicture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        userPicture.CopyTo(fileStream);
                    }

                   
                }

                user.UserName = userViewModel.UserName;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.Email = userViewModel.Email;
                user.City = userViewModel.City;
                user.BirthDay = userViewModel.BirthDay;
                user.Gender = (int)userViewModel.Gender;
                user.Picture = uniqueFileName;
                IdentityResult result = await userManager.UpdateAsync(user);

                if(result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.SignInAsync(user, true);
                    ViewBag.success = "true";
                }
                else
                {
                    AddModelError(result);
                }
            }

            return View(userViewModel);
        }
        public IActionResult PasswordChange()
        {
            
            return View();
        }
        
        [HttpPost]
        public IActionResult PasswordChange(PasswordChangeViewModel passwordChangeViewModel)
        {
            if(ModelState.IsValid)
            {
                AppUser user = currentUser; 
                if(user!=null)
                {
                    bool exist = userManager.CheckPasswordAsync(user, passwordChangeViewModel.OldPassword).Result;
                    if(exist)
                    {
                        IdentityResult result= userManager.ChangePasswordAsync(user,passwordChangeViewModel.OldPassword,passwordChangeViewModel.NewPassword).Result;
                        if(result.Succeeded)
                        {
                            userManager.UpdateSecurityStampAsync(user);

                            signInManager.SignOutAsync();
                            signInManager.PasswordSignInAsync(user, passwordChangeViewModel.NewPassword, true, false);
                            ViewBag.success = "true";
                        }
                        else
                        {
                            AddModelError(result);

                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Eski şifreniz yanlış");
                    }
                }
            }
            return View(passwordChangeViewModel);
        }

        public void LogOut()
        {
            signInManager.SignOutAsync();
        }

        public IActionResult AccessDenied(string returnUrl)
        {
            if(returnUrl.ToLower().Contains("violencePage"))
            {
                ViewBag.message = "Erişmeye çalıştığınız sayfa şiddet videoları içerdiğinden dolayı 15 yaşından büyük olmanız gerekmektedir.";
            }
            else if(returnUrl.ToLower().Contains("ankarapage"))
            {
                ViewBag.message = "Bu sayfaya sadece şehir alanı Ankara olan kullanıcılar erişebilir";
            }
            else if (returnUrl.ToLower().Contains("exchange"))
            {
                ViewBag.message = "30 günlük deneme hakkınız sona ermiştir.";

            }
            else
            {
                ViewBag.message = "Bu sayfa erişim izniniz yoktur. Erişim izni almak için site yöneticisiyle görüşün";
            }
            return View();
        }
        
        [Authorize(Roles="manager,admin")]
        public IActionResult Manager()
        {
            return View();
        }

        [Authorize(Roles = "editor,admin")]
        public IActionResult Editor()
        {
            return View();
        }

        [Authorize(Policy ="AnkaraPolicy")]
        public IActionResult AnkaraPage()
        {
            return View();
        }

        [Authorize(Policy = "ViolencePolicy")]
        public IActionResult ViolencePage()
        {
            return View();
        }

        public async Task<IActionResult> ExchangeRedirect()
        {
            bool result = User.HasClaim(x => x.Type == "ExpireDateExchange");
            if(!result)
            {
                Claim ExpireDateExchange = new Claim("ExpireDateExchange", DateTime.Now.AddDays(30).Date.ToShortDateString(),
                 ClaimValueTypes.String, "Internal");
                await userManager.AddClaimAsync(currentUser, ExpireDateExchange);
                await signInManager.SignOutAsync();
                await signInManager.SignInAsync(currentUser, true);
            }
            return RedirectToAction("Exchange");
        }

        [Authorize(Policy = "ExchangePolicy")]
        public IActionResult Exchange()
        {
            return View();
        }
    }
}
