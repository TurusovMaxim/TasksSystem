using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ClassLibrary.Data;
using TasksSystem.ViewModels;
using TasksSystem.Models;
using TasksSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;
using System.Security.Cryptography;
using TasksSystem.Encryption;

namespace TasksSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AccountController));

        public AccountController(UserService userService, RoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Birthday.Date > DateTime.Now)
            {
                log.Error("Birthday cannot be later than current date");
                return Register();
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByEmail(model.Email);

                var getAdmin = await _userService.GetUserByEmail(User.Identity.Name);

                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email,
                        Password = model.Password,
                    };

                    DecryptedUser decryptedUser = new DecryptedUser
                    {
                        DecryptedFirstName = model.FirstName,
                        DecryptedLastName = model.LastName,
                        Birthday = model.Birthday,
                        DecryptedComment = model.Comment
                    };

                    if (getAdmin == null)
                    {
                        var userRole = await _roleService.GetRoleByName("User");
                        user.Role = userRole;
                    }
                    else
                    {
                        var adminRole = getAdmin.Role.Name;
                        var userRole = await _roleService.GetRoleByName(adminRole);
                        user.Role = userRole;
                    }
                    

                    user.Password = BC.HashPassword(model.Password);

                    var encryptedUser = _userService.GetEncryptedUser(decryptedUser, user);

                    await _userService.CreateUser(encryptedUser);

                    await Authenticate(encryptedUser);

                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect email address");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByEmail(model.Email);

                if(user == null)
                {
                    log.Error("You are not registred!");
                    return View(model);
                }

                bool checkPassword = BC.Verify(model.Password, user.Password);
                if (checkPassword)
                {
                    var login = await _userService.GetUserByEmailAndPwd(model.Email, user.Password);

                    if (login != null)
                    {
                        await Authenticate(login);

                        return RedirectToAction("Index", "Profile");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect email address or password");
                }
            }
            return View(model);
        }


        private async System.Threading.Tasks.Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
