using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TasksSystem.Models;

namespace TasksSystem.Models
{
    public class RoleDb
    {
        [Key]
        public string Name { get; set; }

        public List<UserDb> Users { get; set; }

        public RoleDb()
        {
            Users = new List<UserDb>();
        }

    }
}
