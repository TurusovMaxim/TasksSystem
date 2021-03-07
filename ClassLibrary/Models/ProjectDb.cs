using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.Models
{
    public class ProjectDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskDb> Tasks { get; set; }

        public List<ProjectUsersDb> Users { get; set; }

        [EnumDataType(typeof(StatusesDb))]
        public StatusesDb Status { get; set; }

        public string Text { get; set; }
    }
}
