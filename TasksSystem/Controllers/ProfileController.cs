using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksSystem.Services;
using TasksSystem.Models;
using TasksSystem.Encryption;
using TasksSystem.ViewModels;
using System.Security.Claims;
using Task = TasksSystem.Models.Task;

namespace TasksSystem.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ProfileController : Controller
    {
        private readonly UserService _userService;
        private readonly ProjectService _projectService;

        public ProfileController(UserService userService, ProjectService projectService)
        {
            _userService = userService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserByEmail(User.Identity.Name);

            var decryptedUser = _userService.GetDectyptedUser(user);

            decryptedUser.DecryptedBirthday = DateTime.Parse(decryptedUser.DecryptedBirthday).ToShortDateString();

            decryptedUser.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            //Projects
            List<ProjectUsers> myProjects = decryptedUser.Projects;

            List<Project> projects = new List<Project>();

            foreach(ProjectUsers projectUsers in myProjects.ToList())
            {
                int projectId = projectUsers.ProjectId;

                var project = await _projectService.GetProjectById(projectId);

                projects.Add(project);
            }

            ViewBag.Projects = projects;

            //Tasks
            List<Task> myTasks = decryptedUser.Tasks;

            List<Task> sortedTasks = myTasks.OrderByDescending(o => o.DeadlineDate).ToList();

            foreach(Task task in sortedTasks)
            {
                if(task.DeadlineDate < DateTime.Now.Date)
                {
                    task.isOutstanding = true;
                }
            }

            ViewBag.Tasks = sortedTasks;

            return View(decryptedUser);
        }
    }
}
