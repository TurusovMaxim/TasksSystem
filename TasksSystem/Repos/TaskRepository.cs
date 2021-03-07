using AutoMapper;
using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using TasksSystem.Models;

namespace TasksSystem.Repos
{
    public class TaskRepository
    {
        private readonly ClassLibraryContext _context;
        private readonly IMapper _mapper;

        public TaskRepository(ClassLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task ChangeStatus(Task task, Statuses status)
        {
            task.Status = status;
            _context.Task.Update(_mapper.Map<Task, TaskDb>(task));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task CreateTask(Task task, string userEmail, int projectId, int taskCreatorId)
        {
            var user = await _context.User.FirstOrDefaultAsync(c => c.Email.Equals(userEmail));
            var project = await _context.Project.FirstOrDefaultAsync(c => c.Id == projectId);
            var taskDb = _mapper.Map<Task, TaskDb>(task);
            taskDb.User = user;
            taskDb.Project = project;
            taskDb.taskCreatorId = taskCreatorId;
            _context.Task.Add(taskDb);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Update(Task task)
        {
            _context.Task.Update(_mapper.Map<Task, TaskDb>(task));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<List<Task>> GetAllTasks()
        {
            var tasks = _mapper.Map<List<TaskDb>, List<Task>>(await _context.Task.AsNoTracking().ToListAsync());
            return tasks;
        }

        public async System.Threading.Tasks.Task<Task> GetTaskById(int? id)
        {
            var task = _mapper.Map<TaskDb, Task>(await _context.Task.Include(t => t.Project).Include(t => t.User).Include(t => t.Comments).Include(t => t.User).
                                                                                  AsNoTracking().
                                                                                  FirstOrDefaultAsync(m => m.Id == id));
            return task;
        }

        public async System.Threading.Tasks.Task Remove(Task task)
        {
            _context.Task.Remove(_mapper.Map<Task, TaskDb>(task));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<bool> TaskExists(int? id)
        {
            return await _context.Task.AnyAsync(e => e.Id == id);
        }
    }
}
