using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TasksSystem.Models
{
    public class Task
    {
        public Task(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public Task() { }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime DeadlineDate { get; set; }

        [EnumDataType(typeof(Statuses))]
        public Statuses Status { get; set; }

        [UIHint("tinymce_jguery_full"), AllowHtml]
        public string Text { get; set; }

        public int taskCreatorId { get; set; }

        public bool isOutstanding { get; set; }

        //_________________________________________________

        public Project Project { get; set; }

        public List<Comment> Comments { get; set; }
        public User User { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Task task &&
                   Id == task.Id &&
                   Title == task.Title;
        }
    }
}
