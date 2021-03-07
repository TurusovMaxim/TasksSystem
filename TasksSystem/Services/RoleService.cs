using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Models;
using TasksSystem.Repos;

namespace TasksSystem.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepo;

        public RoleService(RoleRepository roleRepository)
        {
            _roleRepo = roleRepository;
        }

        public async Task<Role> GetRoleByName(string name)
        {
            return await _roleRepo.GetRoleByName(name);
        }
    }
}
