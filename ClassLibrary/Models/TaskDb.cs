using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TasksSystem.Models
{
    public class TaskDb
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DeadlineDate { get; set; }

        [EnumDataType(typeof(StatusesDb))]
        public StatusesDb Status { get; set; }

        [UIHint("tinymce_jguery_full"), AllowHtml]
        public string Text { get; set; }

        public int taskCreatorId { get; set; }

        //_________________________________________________

        public ProjectDb Project { get; set; }

        public List<CommentDb> Comments { get; set; }
        public UserDb User { get; set; }
    }
}
