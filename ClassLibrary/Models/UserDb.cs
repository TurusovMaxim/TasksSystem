using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.Models
{
    public class UserDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public byte[] encryptedFirstName { get; set; }

        public byte[] encryptedLastName { get; set; }

        public byte[] encryptedBirthday { get; set; }

        public byte[] encryptedComment { get; set; }

        public byte[] AesKey { get; set; }

        public byte[] AesIV { get; set; }

        //__________________________________________

        public RoleDb Role { get; set; }

        public List<ProjectUsersDb> Projects { get; set; }

        public List<CommentDb> Comments { get; set; }
        public List<TaskDb> Tasks { get; set; }
    }
}
