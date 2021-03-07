using AutoMapper;
using ClassLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TasksSystem.Models;
using TasksSystem.Encryption;
using System.Security.Cryptography;

namespace TasksSystem.Repos
{
    public class UserRepository
    {
        private readonly ClassLibraryContext _context;
        private readonly TaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public UserRepository(ClassLibraryContext context, TaskRepository taskRepository, IMapper mapper)
        {
            _context = context;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task UpdateEntity(User user)
        {
            _context.User.Update(_mapper.Map<User, UserDb>(user));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task RemoveUser(User user)
        {
            _context.User.Remove(_mapper.Map<User, UserDb>(user));
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task CreateUser(User user)
        {
            var userDb = _mapper.Map<User, UserDb>(user);
            userDb.Role = _context.Role.Find(userDb.Role.Name);
            _context.User.Add(userDb);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<bool> UserExistsAsync(int id)
        {
            return await _context.User.AnyAsync(e => e.Id == id);
        }

        public async System.Threading.Tasks.Task<List<User>> GetAllUsers()
        {
            var users = _mapper.Map<List<UserDb>, List<User>>(await _context.User.Include(u => u.Role).AsNoTracking().ToListAsync());
            return users;
        }

        public async System.Threading.Tasks.Task<User> GetUserById(int? id)
        {
            var user = await _context.User.Include(u => u.Tasks).Include(u => u.Projects).Include(u => u.Role).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<UserDb, User>(user);
        }

        public async System.Threading.Tasks.Task<User> GetUserByEmail(string? Email)
        {
            var user = await _context.User.Include(u => u.Tasks).Include(u => u.Projects).Include(u => u.Role).AsNoTracking().FirstOrDefaultAsync(p => p.Email == Email);
            return _mapper.Map<UserDb, User>(user);
        }




        public async System.Threading.Tasks.Task<User> GetUserByEmailAndPwd(string? Email, string? Password)
        {
            var user = await _context.User.Include(u => u.Tasks).Include(u => u.Role).AsNoTracking().FirstOrDefaultAsync(p => p.Email == Email && p.Password == Password);
            return _mapper.Map<UserDb, User>(user);
        }

        public User GetEncryptedUser(DecryptedUser decryptedUser, User user)
        {
            using (Aes myAes = Aes.Create())
            {
                user.AesKey = myAes.Key;
                user.AesIV = myAes.IV;

                var myKey = user.AesKey;
                var myIV = user.AesIV;

                user.EncryptedFirstName = EncryptDecrypt.EncryptStringToBytes_Aes(decryptedUser.DecryptedFirstName, myKey, myIV);

                user.EncryptedLastName = EncryptDecrypt.EncryptStringToBytes_Aes(decryptedUser.DecryptedLastName, myKey, myIV);

                user.EncryptedBirthday = EncryptDecrypt.EncryptStringToBytes_Aes(decryptedUser.Birthday.ToString(), myKey, myIV);

                user.EncryptedComment = EncryptDecrypt.EncryptStringToBytes_Aes(decryptedUser.DecryptedComment, myKey, myIV);

                return user;
            }
        }

        public DecryptedUser GetDectyptedUser(User user)
        {
            DecryptedUser decryptedUser = new DecryptedUser
            {
                Id = user.Id,
                Email = user.Email,
                DecryptedFirstName = EncryptDecrypt.DecryptStringFromBytes_Aes(user.EncryptedFirstName, user.AesKey, user.AesIV),
                DecryptedLastName = EncryptDecrypt.DecryptStringFromBytes_Aes(user.EncryptedLastName, user.AesKey, user.AesIV),
                DecryptedBirthday = EncryptDecrypt.DecryptStringFromBytes_Aes(user.EncryptedBirthday, user.AesKey, user.AesIV),
                DecryptedComment = EncryptDecrypt.DecryptStringFromBytes_Aes(user.EncryptedComment, user.AesKey, user.AesIV),
                Tasks = user.Tasks,
                Projects = user.Projects
            };

            return decryptedUser;
        }

        public async System.Threading.Tasks.Task<List<Task>> GetUsersTasks(User user)
        {
            var userDb = _mapper.Map<User, UserDb>(user);
            var usersTasks = await _context.Task
                .AsNoTracking()
                .Where(t => t.User.Equals(userDb))
                .ToListAsync();
            return _mapper.Map<List<TaskDb>, List<Task>>(usersTasks);
        }

       
    }
}
