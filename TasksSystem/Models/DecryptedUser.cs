using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.Models
{
    public class DecryptedUser
    {
        public int Id { get; set; }

        public string Email { get; set;}

        public string DecryptedFirstName { get; set; }
        
        public string DecryptedLastName { get; set; }

        public string DecryptedBirthday { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public string DecryptedComment { get; set; }

        public string Role { get; set; }

        public List<Task> Tasks { get; set; }

        public List<ProjectUsers> Projects { get; set; }
    }
}
