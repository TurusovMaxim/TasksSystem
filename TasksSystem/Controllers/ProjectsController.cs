using AutoMapper;
using ClassLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Encryption;
using TasksSystem.Models;
using TasksSystem.Repos;
using TasksSystem.Services;
using TasksSystem.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TasksSystem.Controllers
{
    
    public class ProjectsController : Controller
    {
        private readonly ProjectService _projectService;
        private readonly UserService _userService;
        private readonly ProjectUsersService _projectUsersService;
        private readonly TaskService _taskService;
        private readonly CommentService _commentService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProjectsController));

        private readonly string admin = "Admin";
        private readonly string guest = "Guest";

        public ProjectsController(ProjectService projectService, UserService userService, ProjectUsersService projectUsersService,
                                  TaskService taskService, CommentService commentService)
        {
            _projectService = projectService;
            _userService = userService;
            _projectUsersService = projectUsersService;
            _taskService = taskService;
            _commentService = commentService;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _projectService.GetAllProjects());
        }

        // POST: Tasks/CreateComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(string text, int Id)
        {
            var userEmail = User.Identity.Name;
            if (text == null)
            {
                log.Error("Text of the comment is null");
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                Comment comment = new Comment(text, DateTime.Now);
                await _commentService.CreateComment(comment, userEmail, Id);
                return await TaskDetails(Id);
                //return RedirectToAction("Index", "Profile");
            }
            return View();
        }


        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreateTask(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                log.Error("Project with given id doesn't exist");
                return NotFound();
            }
            List<User> users = await _projectService.GetProjectsUsers(id);

            foreach (User user in users)
            {
                var getUser = await _userService.GetUserById(user.Id);

                if (getUser.Email == User.Identity.Name || User.IsInRole(admin))
                {
                    ViewBag.Users = users;
                    return View(project);
                }
            }

            return RedirectToAction("Details", "Projects", project);
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(string Title, string Text, DateTime DeadlineDate, string userEmail, int Id)
        {
            Models.Task task = new Models.Task(Title, Text);

            if (task.Text == null || task.Title == null || userEmail == null)
            {
                log.Error("Task's text or title or user are null");
                return await CreateTask(Id);
            }

            if (DeadlineDate.Date < DateTime.Today.Date)
            {
                log.Error("Deadline date can't be earlier than current day");
                return await CreateTask(Id);
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByEmail(User.Identity.Name);
                int taskCreatorId = user.Id;
                await _taskService.Create(task, DeadlineDate, userEmail, Id, taskCreatorId);

                return await Details(Id);
            }
            return View();
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            List<User> users = await _userService.GetAllUsers();

            ViewBag.Users = users;

            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Project project, string[] users)
        {
            if (users.Length >= 4 || users.Length == 0)
            {
                log.Error("More than 3 users!");
                return await Create();
            }
            else if (project.Name == null)
            {
                log.Error("Null project's name");
                return await Create();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _projectService.CreateProject(project, users);
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
                if (id == null)
                {
                    log.Error("Id cannot be null");
                    return NotFound();
                }

                var project = await _projectService.GetProjectById(id);
                if (project == null)
                {
                    log.Error("Project with given id doesn't exist");
                    return NotFound();
                }
                ViewBag.Statuses = Enum.GetValues(typeof(Statuses));
                
            
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Project project, string Status)
        {

            if (project.Name == null)
            {
                log.Error("Null project's name");
                return await Edit(id);
            }

            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _projectService.Edit(project, Status.ToLower());
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                log.Error("Project with given id doesn't exist");
                return NotFound();
            }

            List<User> users = await _projectService.GetProjectsUsers(id);

            ViewBag.Users = users;
            List<Models.Task> tasks = project.Tasks;
            ViewBag.Tasks = tasks;
            return View("Details", project);
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
                log.Error("Task with given id doesn't exist");
                return NotFound();
            }

            var comments = await _commentService.GetAllTasksComments(task.Id);
            comments.Sort((Comment c1, Comment c2) => c2.CommentDate.CompareTo(c1.CommentDate));
            ViewBag.Comments = comments;

            if (task.DeadlineDate.Date > DateTime.Now.Date)
            {
                ViewBag.DaysLeft = task.DeadlineDate.Date.Subtract(DateTime.Now.Date).TotalDays;
            }
            else
            {
                ViewBag.DaysLeft = 0;
            }

            ViewBag.DaysTotal = task.DeadlineDate.Date.Subtract(task.CreationDate.Date).TotalDays;
            ViewBag.CreationDate = task.CreationDate.ToShortDateString();
            ViewBag.DeadlineDate = task.DeadlineDate.ToShortDateString();
            return View("TaskDetails", task);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> UsersDetails(int? id)
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

            var decryptUser = _userService.GetDectyptedUser(user);
            ViewBag.Projects = user.Projects;
            return View(decryptUser);
        }

        // GET: Tasks/EditTask/5
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> EditTask(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                log.Error("Task with given id doesn't exist");
                return NotFound();
            }

            if(User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserByEmail(User.Identity.Name);

                if (task.taskCreatorId == user.Id || User.IsInRole(admin))
                {
                    ViewBag.Statuses = Enum.GetValues(typeof(Statuses));
                    return View(task);
                }
            }

            return RedirectToAction("TaskDetails", "Projects", task);
        }

        // POST: Tasks/EditTask/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int id, [FromForm] Models.Task task, string Status)
        {
            if (task.Text == null || task.Title == null)
            {
                log.Error("Task's text or title are null");
                return await EditTask(id);
            }

            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var getTask = await _taskService.GetTaskById(id);
                getTask.Title = task.Title;
                getTask.Text = task.Text;
                getTask.Status = task.Status;

                await _taskService.Update(getTask);
                return RedirectToAction("Index", "Profile");
            }
            return await Index();
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
                if (id == null)
                {
                    log.Error("Id cannot be null");
                    return NotFound();
                }

                var project = await _projectService.GetProjectById(id);
                if (project == null)
                {
                    log.Error("Project with given id doesn't exist");
                    return NotFound();
                }

                return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _projectService.GetProjectById(id);
            await _projectService.RemoveProject(project);
            return RedirectToAction(nameof(Index));
        }

        private async System.Threading.Tasks.Task<bool> PojectExists(int id)
        {
            return await _projectService.ProjectExists(id);
        }

        // GET: Projects/DeleteComment/5
        public async Task<IActionResult> DeleteComment(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var comment = await _commentService.GetCommentById(id);
            if (comment == null)
            {
                log.Error("There's no comment with given id");
                return NotFound();
            }


            return View(comment);
           
        }

        // POST: Projects/DeleteComment/5
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCommentConfirmed(int id)
        {
            var comment = await _commentService.GetCommentById(id);
            await _commentService.RemoveComment(comment);
            return RedirectToAction("Index", "Profile");
        }

        // GET: Projects/DeleteTask/5
        public async Task<IActionResult> DeleteTask(int? id)
        {
            if (id == null)
            {
                log.Error("Id cannot be null");
                return NotFound();
            }

            var task = await _taskService.GetTaskById(id);
            if (task == null)
            {
                log.Error("There's no task with given id");
                return NotFound();
            }

            if(User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserByEmail(User.Identity.Name);

                if (task.taskCreatorId == user.Id || User.IsInRole(admin))
                {
                    return View(task);
                }
            }
            
            return RedirectToAction("TaskDetails", "Projects", task);
        }

        // POST: Projects/DeleteTask/5
        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTaskConfirmed(int id)
        {
            var task = await _taskService.GetTaskById(id);
            await _taskService.Remove(task);
            return RedirectToAction("Index", "Profile");
        }
    }
}