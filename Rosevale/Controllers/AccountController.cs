using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Rosevale.DAL;
using Rosevale.Helpers;
using Rosevale.Models;
using Rosevale.ViewModels;

namespace Rosevale.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser> signInManager,
                                 IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (string.IsNullOrEmpty(loginVM.Username))
            {
                ModelState.AddModelError("Username", "Username or Email is required");
                return View(loginVM);
            }

            if (string.IsNullOrEmpty(loginVM.Password))
            {
                ModelState.AddModelError("Password", "Password is required");
                return View(loginVM);
            }

            AppUser appUser = await _userManager.FindByNameAsync(loginVM.Username);
            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(loginVM.Username);
                if (appUser == null)
                {
                    ModelState.AddModelError("Username", "Account is not found");
                    return View(loginVM);
                }
            }

            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("Username", "Account is deactivated");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.IsRemember, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Max 5 attempts exceeded. Account is locked.");
                return View(loginVM);
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Password", "Password is incorrect");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (string.IsNullOrEmpty(registerVM.Name))
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(registerVM);
            }

            if (string.IsNullOrEmpty(registerVM.Surname))
            {
                ModelState.AddModelError("Surname", "Surname is required");
                return View(registerVM);
            }

            if (string.IsNullOrEmpty(registerVM.Username))
            {
                ModelState.AddModelError("Username", "Username is required");
                return View(registerVM);
            }

            if (string.IsNullOrEmpty(registerVM.Email))
            {
                ModelState.AddModelError("Email", "Email is required");
                return View(registerVM);
            }

            if (string.IsNullOrEmpty(registerVM.Password))
            {
                ModelState.AddModelError("Password", "Password is required");
                return View(registerVM);
            }

            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(appUser, Roles.Member);
            await _signInManager.SignInAsync(appUser, registerVM.IsRemember);
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region CreateRoles
        //public async Task CreateRoles()
        //{
        //    if(!await _roleManager.RoleExistsAsync(Roles.Admin))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin});
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Roles.Staff))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Staff });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Roles.Member))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Member });
        //    }
        //}
        #endregion

        #region ForgotPassword

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync("fidan.tomarova@gmail.com", model.Email, "Reset Password", $"Please reset your password by clicking <a href='{callbackUrl}'>here</a>.");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region ResetPassword

        public IActionResult ResetPassword(string token = null)
        {
            return View(new ResetPasswordFromLoginVM { Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordFromLoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
    }
}

