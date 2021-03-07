using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Models;
using TasksSystem.Repos;

namespace TasksSystem.Services
{
    public class ProjectUsersService
    {
        ProjectUsersRepository _projectUsersRepository;
        public ProjectUsersService(ProjectUsersRepository projectUsersRepository)
        {
            _projectUsersRepository = projectUsersRepository;
        }

        public async System.Threading.Tasks.Task<List<ProjectUsers>> GetAll()
        {
            return await _projectUsersRepository.GetAll();
        }
    }
}
