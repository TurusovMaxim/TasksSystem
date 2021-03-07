using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.Models
{
    public class CommentDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String text { get; set; }

        public DateTime CommentDate { get; set; }

        public UserDb User { get; set; }

        public TaskDb Task { get; set; }
    }
}
