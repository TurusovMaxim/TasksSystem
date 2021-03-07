using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.Models
{
    public class Role
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }

        public Role()
        {
            Users = new List<User>();
        }
    }
}
