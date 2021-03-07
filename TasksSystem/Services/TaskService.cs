using System;
using System.Collections.Generic;
using System.Linq;
using TasksSystem.Models;
using TasksSystem.Repos;

namespace TasksSystem.Services
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        public TaskService(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task ChangeStatus(Task task, string status)
        {
            switch (status)
            {
                case "open":
                    await _taskRepository.ChangeStatus(task, Statuses.Open);
                    break;
                case "done":
                    await _taskRepository.ChangeStatus(task, Statuses.Done);
                    break;
                case "closed":
                    await _taskRepository.ChangeStatus(task, Statuses.Closed);
                    break;
            }
        }

        public async System.Threading.Tasks.Task<List<Task>> GetAllTasks()
        {
            return await _taskRepository.GetAllTasks();
        }

        public async System.Threading.Tasks.Task<Task> GetTaskById(int? id)
        {
            return await _taskRepository.GetTaskById(id);
        }

        public async System.Threading.Tasks.Task<bool> TaskExists(int? id)
        {
            return await _taskRepository.TaskExists(id);
        }

        public async System.Threading.Tasks.Task Remove(Task task)
        {
            await _taskRepository.Remove(task);
        }

        public async System.Threading.Tasks.Task Create(Task task, DateTime DeadlineDate, string userEmail, int projectId, int taskCreatorId)
        {
            task.CreationDate = DateTime.Now;
            task.DeadlineDate = DeadlineDate;
            await _taskRepository.CreateTask(task, userEmail, projectId, taskCreatorId);
        }

        public async System.Threading.Tasks.Task Update(Task task)
        {
            await _taskRepository.Update(task);
        }
    }
}
