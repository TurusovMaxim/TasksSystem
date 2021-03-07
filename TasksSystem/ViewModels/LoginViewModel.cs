using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TasksSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
