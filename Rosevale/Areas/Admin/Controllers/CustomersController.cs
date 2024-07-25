using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rosevale.Helpers;
using Rosevale.Models;
using Rosevale.ViewModels;

namespace Rosevale.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Staff")]
    public class CustomersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public CustomersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.OrderByDescending(x => x.Id).ToListAsync();
            List<UserVM> usersVM = new List<UserVM>();
            foreach (AppUser user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Member"))
                {
                    UserVM userVM = new UserVM
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Surname = user.Surname,
                        Username = user.UserName,
                        Email = user.Email,
                        IsDeactive = user.IsDeactive,
                        Role = (await _userManager.GetRolesAsync(user))[0],
                    };
                    usersVM.Add(userVM);
                }
            }
            return View(usersVM);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Member,
            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterVM registerVM, string role)
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Member,
            };
            AppUser appUser = new AppUser
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
            await _userManager.AddToRoleAsync(appUser, role);
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            AppUser? appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return RedirectToAction("BadRequest");
            }
            EditVM dbEditVM = new EditVM
            {
                Name = appUser.Name,
                Surname = appUser.Surname,
                Username = appUser.UserName,
                Email = appUser.Email,
                Role = (await _userManager.GetRolesAsync(appUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Member,
            };
            return View(dbEditVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditVM editVM, string id, string role)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound");
            }
            AppUser? appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return RedirectToAction("BadRequest");
            }
            EditVM dbEditVM = new EditVM
            {
                Name = appUser.Name,
                Surname = appUser.Surname,
                Username = appUser.UserName,
                Email = appUser.Email,
                Role = (await _userManager.GetRolesAsync(appUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Member,
            };
            appUser.Name = editVM.Name;
            appUser.Surname = editVM.Surname;
            appUser.UserName = editVM.Username;
            appUser.Email = editVM.Email;
            if (dbEditVM.Role != role)
            {
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(appUser, role);
                if (!addIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(dbEditVM);
                }
                IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(appUser, dbEditVM.Role);
                if (!removeIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(dbEditVM);
                }
            }
            await _userManager.UpdateAsync(appUser);
            return RedirectToAction("Index");
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest");
            }
            AppUser? appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return RedirectToAction("NotFound");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordVM resetPasswordVM, string id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest");
            }
            AppUser? appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return RedirectToAction("NotFound");
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            IdentityResult identityResult = await _userManager.ResetPasswordAsync(appUser, token, resetPasswordVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest");
            }
            AppUser? appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return RedirectToAction("BadRequest");
            }
            if (appUser.IsDeactive)
            {
                appUser.IsDeactive = false;
            }
            else
            {
                appUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(appUser);
            return RedirectToAction("Index");
        }
        #endregion

        #region ErrorPages
        public IActionResult BadRequest()
        {
            return View();
        }
        public IActionResult NotFound()
        {
            return View();
        }
        #endregion
    }
}
