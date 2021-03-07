using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary;
using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using TasksSystem.Models;
using TasksSystem.Services;
using TasksSystem.Encryption;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;


namespace TasksSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly TaskService _taskService;
        private readonly RoleService _roleService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UsersController));

        public UsersController(UserService userService, TaskService taskService, RoleService roleService)
        {
            _userService = userService;
            _taskService = taskService;
            _roleService = roleService;
        }
        public async Task<IActionResult> Index()
        {
            List<User> myList = await _userService.GetAllUsers();

            List<DecryptedUser> decryptedUsers = new List<DecryptedUser>();
            
            foreach(User user in myList)
            {
                var getUser = await _userService.GetUserById(user.Id);
                var decryptUser = _userService.GetDectyptedUser(getUser);
                decryptedUsers.Add(decryptUser);
            }

            return View(decryptedUsers);
        }

        public async Task<IActionResult> TaskDetails(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                log.Error("Task with given id was't found");
                return NotFound();
            }

            return View(task);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
           return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DecryptedUser decryptedUser)
        {
            if (decryptedUser.DecryptedFirstName == null)
            {
                log.Error("User's name is null!");
                return Create();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    User user = new User();

                    var encryptUser = _userService.GetEncryptedUser(decryptedUser, user);

                    var userRole = await _roleService.GetRoleByName(decryptedUser.Role);
                    if (userRole != null)
                    {
                        encryptUser.Role = userRole;
                    }

                    await _userService.CreateUser(encryptUser);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                log.Error("User with given id wasn't foung");
                return NotFound();
            }

            var decryptUser = _userService.GetDectyptedUser(user);
            
            return View(decryptUser);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] DecryptedUser user)
        {
            if (user.DecryptedFirstName == null)
            {
                log.Error("User's name is null!");
                return await Edit(id);
            }

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var getUser = await _userService.GetUserById(user.Id);

                    var decryptedUser = _userService.GetDectyptedUser(getUser);

                    decryptedUser.DecryptedFirstName = user.DecryptedFirstName;

                    decryptedUser.DecryptedLastName = user.DecryptedLastName;

                    var encryptedUser = _userService.GetEncryptedUser(decryptedUser, getUser);

                    await _userService.Edit(encryptedUser);    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                log.Error("User with given id doesn't exist");
                return NotFound();
            }

            var decryptedUser = _userService.GetDectyptedUser(user);

            ViewBag.Tasks = decryptedUser.Tasks;
            return View(decryptedUser);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                log.Error("User with given id doesn't exist");
                return NotFound();
            }

            var decryptedUser = _userService.GetDectyptedUser(user);

            return View(decryptedUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userService.GetUserById(id);
            await _userService.RemoveUser(user);
            return RedirectToAction(nameof(Index));
        }

        private async System.Threading.Tasks.Task<bool> UserExists(int id)
        {
            return await _userService.UserExists(id);
        }
    }
}
