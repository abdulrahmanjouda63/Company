using System.Security.Claims;
using System.Threading.Tasks;
using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using MailKit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IMailService = Company.PL.Helpers.IMailService;

namespace Company.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly ITwilioService _twilioService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, ITwilioService twilioService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _twilioService = twilioService;
        }
        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is null)
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            PhoneNumber = model.PhoneNumber,
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                ModelState.AddModelError("", "User Name Or Email Already Exist");
            }
            return View(model);
        }


        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.IsLockedOut)
                        {
                            ModelState.AddModelError("", "User Is Locked Out");
                        }
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                        if (result.IsNotAllowed)
                        {
                            ModelState.AddModelError("", "User Is Not Allowed");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Email Or Password");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties()
            {
                RedirectUri = Url.Action(nameof(GoogleResponse))
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
                claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value
                };
                await _userManager.CreateAsync(user);
            }
            else
            {
                user.FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                user.LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        //public IActionResult FacebookLogin()
        //{
        //    var prop = new AuthenticationProperties()
        //    {
        //        RedirectUri = Url.Action(nameof(FacebookResponse))
        //    };
        //    return Challenge(prop, FacebookDefaults.AuthenticationScheme);
        //}

        //public async Task<IActionResult> FacebookResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
        //    var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
        //        claim => new {
        //            claim.Issuer,
        //            claim.OriginalIssuer,
        //            claim.Type,
        //            claim.Value
        //        });

        //    var user = await _userManager.GetUserAsync(User);
        //    if (user != null)
        //    {
        //        user.FirstName = claims.FirstOrDefault(c => c.Type == "first_name")?.Value;
        //        user.LastName = claims.FirstOrDefault(c => c.Type == "last_name")?.Value;
        //        await _userManager.UpdateAsync(user);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        #endregion

        #region SignOut

        [HttpGet]
        public new async Task<IActionResult> SignOut()

        {

            await _signInManager.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return RedirectToAction(nameof(SignIn));

        }


        #endregion

        #region ForgotPassword

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPassword(string email, string resetMethod)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action("ResetPassword", "Account", new { email, token }, Request.Scheme);

                    if (resetMethod == "Email")
                    {
                        var emailMessage = new Email()
                        {
                            To = email,
                            Subject = "Reset Password",
                            Body = $"Click the link to reset your password: {url}"
                        };
                        _mailService.SendEmail(emailMessage);

                        // Redirect to CheckYourBox view
                        return RedirectToAction(nameof(CheckYourBox));
                    }
                    else if (resetMethod == "Sms")
                    {
                        var smsMessage = new Sms()
                        {
                            To = user.PhoneNumber,
                            Body = $"Reset your password using this link: {url}"
                        };
                        _twilioService.SendSms(smsMessage);

                        // Redirect to CheckYourPhone view
                        return RedirectToAction(nameof(CheckYourPhone));
                    }
                }
            }

            ModelState.AddModelError("", "Invalid Reset Password Operation!");
            return View("ForgotPassword");
        }


        #endregion

        #region ResetPassword

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;
                if (email is null || token is null) return BadRequest("Invalid Operations");
                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }
                ModelState.AddModelError("", "Invalid Reset Password Operation !!");
            }
            return View();
        }

        #endregion

        [HttpGet]
        public IActionResult CheckYourBox()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CheckYourPhone()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
