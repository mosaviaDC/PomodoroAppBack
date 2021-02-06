using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PomodoroApp.Models
{
    public class User : IdentityUser
    {

        public string firstName { get; set; }

        public List<Tasks> UserTasks { get; set; }

        public int telegramChatId { get; set; }

        public string telegramUserName { get; set; }
    }
    public class Tasks 
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }

        public DateTime TaskDateTime { get; set; }         //Когда завершится задача

        public bool Notify { get; set; }   // Необходимо ли уведомлять
        public int TaskPeriods { get; set; }               //Количество периодов

        public int CurrentPomodoroTime { get; set; }

        public int CurrentPomodoroPauseTime { get; set; }
        public bool InPomodoroPause { get; set; }
        public bool Important { get; set; }
        public int LastPeriodTime { get; set; }
        public bool IsDone { get; set; }
        public string UserId { get; set; }
        public bool inProgress { get; set; }


        
        
    }
}
