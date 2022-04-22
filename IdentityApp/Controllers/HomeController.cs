using IdentityApp.Models;
using IdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityApp.Controllers
{
    public class HomeController : BaseController
    {
       
        public HomeController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager):base(_userManager,_signInManager)
        {
            
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if(ModelState.IsValid)
            {
                AppUser userObj = new AppUser();
                userObj.UserName = userViewModel.UserName;
                userObj.PhoneNumber = userViewModel.PhoneNumber;
                userObj.Email = userViewModel.Email;

                IdentityResult result = await userManager.CreateAsync(userObj, userViewModel.Password);

                if(result.Succeeded)
                {
                    string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(userObj);
                    string link = Url.Action("ConfirmEmail", "Home", new {
                    userId=userObj.Id,
                    token=confirmationToken

                    },protocol:HttpContext.Request.Scheme);

                    Helper.EmailConfirmation.SendEmail(link, userObj.Email); 
                    return RedirectToAction("LogIn");

                }
                else
                {
                    AddModelError(result);
                }
            }
            return View(userViewModel);
        }

        public IActionResult LogIn(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel userLogin)
        {
            if(ModelState.IsValid)
            {
                AppUser user =  await userManager.FindByEmailAsync(userLogin.Email);
                 if(user!=null)
                {
                    if(await userManager.IsLockedOutAsync(user))// if user account is locked
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kitlenmiştir, lütfen daha sonra deneyiniz.");
                    }

                    if(!userManager.IsEmailConfirmedAsync(user).Result)
                    {
                        ModelState.AddModelError("", "E-Posta adresiniz onaylanmamıştır. Lütfen epostanızı kontrol ediniz.");
                        return View(userLogin);
                    }

                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync
                        (user, userLogin.Password, userLogin.RememberMe, false);

                    if(result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user); //if user is successfully login then equal accessfaildaccount to zero
                        if (TempData["ReturnUrl"] != null)
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else // incorrect password
                    {
                        await userManager.AccessFailedAsync(user);// in every wrong password loging increase accessfaild value with +1
                        int fail = await userManager.GetAccessFailedCountAsync(user);  // take accessfaild count from database

                        ModelState.AddModelError("", $"{fail} kez başarısız giriş!"); 
                        
                        if(fail==3)
                        {
                            await userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolay 20 dakika süreyle kitlenmiştir. Lütfen daha sonra tekrar deneyiz.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Geçersiz Kullanıcı adı veya şifre");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz Kullanıcı adı veya şifre");
                }
            }
          
            return View(userLogin);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result; // this "Result" works as awit ot async
            
            if(user!=null)
            {
                string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;  // create token
                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Home", new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme
                );

                Helper.PasswordReset.PasswordResetSendEmail(passwordResetLink,user.Email);
                ViewBag.status = "success";

            }
            else
            {
                ModelState.AddModelError("", "sistemde kayıtlı email adresi bulunamamıştır");
            }
            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPasswordConfirm(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")]PasswordResetViewModel passwordResetViewModel) // we take only PasswordNew property from model by using Bind
        {
            string token = TempData["token"].ToString();
            string userId = TempData["userId"].ToString();

            AppUser user = await userManager.FindByIdAsync(userId);
            if(user!=null)
            {
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);

                if(result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    ViewBag.status = "success"; 
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "hata meydana gelmiştir. Lütfen daha sonra tekrar deneyiniz.");
            }

            return View(passwordResetViewModel);
        }
        

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            IdentityResult result = await userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                ViewBag.status = "E-Posta adresiniz onaylanmıştır. Login ekranına giriş yapabilirsiniz";

            }
            else
            {
                ViewBag.status = "Bir hata meydana geldi. Lütfen daha sonra tekrar denyiniz.";
            }
            return View();
        }

        public IActionResult FacebookLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Facebook", RedirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        public IActionResult GoogleLogin(string ReturnUrl)
        {
            string RedirectUrl = Url.Action("ExternalResponse", "Home", new { ReturnUrl = ReturnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", RedirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> ExternalResponse(string ReturnUrl="/")
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if(info==null)
            {
                return RedirectToAction("Login");

            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                    info.ProviderKey, true);
                if(result.Succeeded)
                {
                    return Redirect(ReturnUrl);

                }
                else
                {
                    AppUser user = new AppUser();
                    user.Email = info.Principal.FindFirst(ClaimTypes.Email).Value;
                    string externalUserId = info.Principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if(info.Principal.HasClaim(x=>x.Type==ClaimTypes.Name))
                    {
                        string userName = info.Principal.FindFirst(ClaimTypes.Name).Value;
                        userName = userName.Replace(' ', '-').ToLower() + externalUserId.Substring(0, 5).ToString();
                        user.UserName = userName;
                    }
                    else
                    {
                        user.UserName = info.Principal.FindFirst(ClaimTypes.Name).Value;
                    }

                    AppUser user2 = await userManager.FindByEmailAsync(user.Email);
                    if(user2==null)
                    {
                        IdentityResult createResult = await userManager.CreateAsync(user);
                        if (createResult.Succeeded)
                        {
                            IdentityResult loginResult = await userManager.AddLoginAsync(user, info);
                            if (loginResult.Succeeded)
                            {
                                //await signInManager.SignInAsync(user, true);
                                await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                AddModelError(loginResult);
                            }
                        }
                        else
                        {
                            AddModelError(createResult);
                        }
                    }
                    else
                    {
                        IdentityResult loginResult = await userManager.AddLoginAsync(user2, info);
                        await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);
                        return Redirect(ReturnUrl);
                    }

                    
                }
            }

            List<string> errors = ModelState.Values.SelectMany(X => X.Errors).Select(y => y.ErrorMessage).ToList();
            return RedirectToAction("Error",errors);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
