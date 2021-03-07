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
    public class ProjectUsersRepository
    {
        private readonly ClassLibraryContext _context;
        private readonly IMapper _mapper;

        public ProjectUsersRepository(ClassLibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task<List<ProjectUsers>> GetAll()
        {
            return _mapper.Map<List<ProjectUsersDb>, List<ProjectUsers>>(await _context.ProjectUsers.AsNoTracking().ToListAsync());
        } 
    }
}
