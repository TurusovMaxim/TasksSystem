using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }

        public List<ProjectUsers> Users { get; set; }

        [EnumDataType(typeof(Statuses))]
        public Statuses Status { get; set; }

        public Project()
        {
            Tasks = new List<Task>();
            Users = new List<ProjectUsers>();
        }

        public override bool Equals(object obj)
        {
            return obj is Project project &&
                   Id == project.Id &&
                   Name == project.Name;
        }
    }
}
