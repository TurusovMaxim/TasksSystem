using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] EncryptedFirstName { get; set; }

        public byte[] EncryptedLastName { get; set; }

        public byte[] EncryptedBirthday { get; set; }

        public byte[] EncryptedComment { get; set; }

        public byte[] AesKey { get; set; }

        public byte[] AesIV { get; set; }

        //__________________________________________

        public Role Role { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Task> Tasks { get; set; }

        public List<ProjectUsers> Projects { get; set; }

        public User()
        {
            Projects = new List<ProjectUsers>();
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id == user.Id;
        }
    }
}
