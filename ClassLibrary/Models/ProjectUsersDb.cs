using System;
using System.Collections.Generic;
using System.Linq;

namespace TasksSystem.Models
{
    public class ProjectUsersDb
    {
        public int ProjectId { get; set; }
        public ProjectDb Project { get; set; }

        public int UserId { get; set; }
        public UserDb User { get; set; }
    }
}
