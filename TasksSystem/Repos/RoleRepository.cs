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


    public class RoleRepository
    {
        private readonly ClassLibraryContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(ClassLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Role> GetRoleByName(string name)
        {
            var role = await _context.Role.FirstOrDefaultAsync(p => p.Name.Equals(name));
            return _mapper.Map<RoleDb, Role>(role);
        }

    }
}
