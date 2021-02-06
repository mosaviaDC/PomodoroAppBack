using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PomodoroApp.Models
{
    public class RegisterModel
    {

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }


    }
}
