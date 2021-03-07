using AutoMapper;
using ClassLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TasksSystem.Models;
using TasksSystem.Repos;

namespace TasksSystem.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;
        private readonly TaskRepository _taskRepo;
        public UserService(ClassLibraryContext context, UserRepository userRepo, TaskRepository taskRepo)
        {
            _userRepo = userRepo;
            _taskRepo = taskRepo;
        }

        public async System.Threading.Tasks.Task RemoveUser(User user)
        {
            await DeleteAllUsersTasks(user);
            await _userRepo.RemoveUser(user);
        }

        public async System.Threading.Tasks.Task CreateUser(User user)
        {
            await _userRepo.CreateUser(user);
        }

        public async System.Threading.Tasks.Task<bool> UserExists(int id)
        {
            return await _userRepo.UserExistsAsync(id);
        }

        public async System.Threading.Tasks.Task<List<User>> GetAllUsers()
        {
            return await _userRepo.GetAllUsers();
        }

        public async System.Threading.Tasks.Task<User> GetUserById(int? id)
        {
            return await _userRepo.GetUserById(id);
        }

        

        public async System.Threading.Tasks.Task<User> GetUserByEmail(string? Email)
        {
            return await _userRepo.GetUserByEmail(Email);
        }

        public async System.Threading.Tasks.Task<User> GetUserByEmailAndPwd(string? Email, string? Password)
        {
            return await _userRepo.GetUserByEmailAndPwd(Email, Password);
        }

        public DecryptedUser GetDectyptedUser(User user)
        {
            return _userRepo.GetDectyptedUser(user);
        }

        public User GetEncryptedUser(DecryptedUser decryptedUser,  User user)
        {
            return _userRepo.GetEncryptedUser(decryptedUser, user);
        }

        public async System.Threading.Tasks.Task Edit(User user)
        {
            await _userRepo.UpdateEntity(user);
        }

        public async System.Threading.Tasks.Task<List<Task>> GetUsersTasks(User user)
        {
            return await _userRepo.GetUsersTasks(user);
        }

        public async System.Threading.Tasks.Task DeleteAllUsersTasks(User user)
        {
            List<Task> tasks = await GetUsersTasks(user);
            foreach (Task task in tasks)
            {
                await _taskRepo.Remove(task);
            }
        }
    }
}
