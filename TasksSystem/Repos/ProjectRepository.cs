using AutoMapper;
using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Models;

namespace TasksSystem.Repos
{
    public class ProjectRepository
    {
        private readonly ClassLibraryContext _context;
        private readonly IMapper _mapper;
        public ProjectRepository(ClassLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task ChangeStatus(Project project, Statuses status)
        {
            project.Status = status;
            _context.Project.Update(_mapper.Map<Project, ProjectDb>(project));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateEntity(Project project)
        {
            _context.Project.Update(_mapper.Map<Project, ProjectDb>(project));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task RemoveProject(Project project)
        {
            _context.Project.Remove(_mapper.Map<Project, ProjectDb>(project));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<bool> ProjectExists(int id)
        {
            return await _context.Project.AnyAsync(e => e.Id == id);
        }

        public async System.Threading.Tasks.Task CreateProject(Project project, string[] users)
        {
            ProjectUsersDb projectUsersDb1 = new ProjectUsersDb();
            ProjectUsersDb projectUsersDb2 = new ProjectUsersDb();
            ProjectUsersDb projectUsersDb3 = new ProjectUsersDb();

            ProjectDb projectDb = _mapper.Map<Project, ProjectDb>(project);

            var userDb1 = await _context.User.AsNoTracking().FirstOrDefaultAsync(c => c.Email.Equals(users[0]));
            projectUsersDb1.UserId = userDb1.Id;
            projectUsersDb1.ProjectId = _mapper.Map<Project, ProjectDb>(project).Id;
            ProjectUsers projectUsers1 = _mapper.Map<ProjectUsersDb, ProjectUsers>(projectUsersDb1);

            if (users.Length > 1)
            {
                var userDb2 = await _context.User.AsNoTracking().FirstOrDefaultAsync(c => c.Email.Equals(users[1]));
                projectUsersDb2.UserId = userDb2.Id;
                projectUsersDb2.ProjectId = _mapper.Map<Project, ProjectDb>(project).Id;
                ProjectUsers projectUsers2 = _mapper.Map<ProjectUsersDb, ProjectUsers>(projectUsersDb2);
                projectDb.Users.Add(projectUsersDb2);
            }
            if (users.Length > 2)
            {
                var userDb3 = await _context.User.AsNoTracking().FirstOrDefaultAsync(c => c.Email.Equals(users[2]));
                projectUsersDb3.UserId = userDb3.Id;
                projectUsersDb3.ProjectId = _mapper.Map<Project, ProjectDb>(project).Id;
                ProjectUsers projectUsers3 = _mapper.Map<ProjectUsersDb, ProjectUsers>(projectUsersDb3);
                projectDb.Users.Add(projectUsersDb3);
            }
            projectDb.Users.Add(projectUsersDb1);


            _context.Project.Add(projectDb);
            _context.UpdateRange();
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<List<Project>> GetAllProjects()
        {
            var projects = _mapper.Map<List<ProjectDb>, List<Project>>(await _context.Project.AsNoTracking().ToListAsync());
            return projects;
        }

        public async System.Threading.Tasks.Task<Project> GetProjectById(int? id)
        {
            var project = _mapper.Map<ProjectDb, Project>(await _context.Project.Include(p => p.Tasks).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id));
            return project;
        }
    }
}
