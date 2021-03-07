using AutoMapper;
using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Models;
using TasksSystem.Repos;

namespace TasksSystem.Services
{
    public class ProjectService

    {
        private readonly ProjectRepository _projectRepo;
        private readonly ProjectUsersRepository _projectUsersRepository;
        private readonly UserRepository _userRepository;

        public ProjectService(ProjectRepository projectRepository,
                              ProjectUsersRepository projectUsersRepository, 
                              UserRepository userRepository)
        {
            _projectRepo = projectRepository;
            _projectUsersRepository = projectUsersRepository;
            _userRepository = userRepository;
        }

        public async System.Threading.Tasks.Task RemoveProject(Project project)
        {
            await _projectRepo.RemoveProject(project);
        }

        public async System.Threading.Tasks.Task<bool> ProjectExists(int id)
        {
            return await _projectRepo.ProjectExists(id);
        }

        public async System.Threading.Tasks.Task<List<User>> GetProjectsUsers(int? id)
        {
            List<User> users = new List<User>();
            var projects = await _projectUsersRepository.GetAll();
            foreach (ProjectUsers projectUsers in projects)
            {
                if (projectUsers.ProjectId == id)
                {
                    users.Add(await _userRepository.GetUserById(projectUsers.UserId));
                }
            }
            return users;
        }

        public async System.Threading.Tasks.Task CreateProject(Project project, string[] users)
        {
            await _projectRepo.CreateProject(project, users);
        }

        public async System.Threading.Tasks.Task<List<Project>> GetAllProjects()
        {
            return await _projectRepo.GetAllProjects();
        }

        public async System.Threading.Tasks.Task<Project> GetProjectById(int? id)
        {
            return await _projectRepo.GetProjectById(id);
        }

        public async System.Threading.Tasks.Task Edit(Project project, string status)
        {
            await _projectRepo.UpdateEntity(project);
        }
    }
}