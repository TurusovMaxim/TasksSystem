using AutoMapper;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasksSystem.Models;

namespace TasksSystem
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDb>();
            CreateMap<UserDb, User>();
            CreateMap<Role, RoleDb>();
            CreateMap<RoleDb, Role>();
            CreateMap<Models.Task, TaskDb>();
            CreateMap<TaskDb, Models.Task>();
            CreateMap<Project, ProjectDb>();
            CreateMap<ProjectDb, Project>();
            CreateMap<ProjectUsersDb, ProjectUsers>();
            CreateMap<ProjectUsers, ProjectUsersDb>();
            CreateMap<CommentDb, Comment>();
            CreateMap<Comment, CommentDb>();
        }
    }
}
