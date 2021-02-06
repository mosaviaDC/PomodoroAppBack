using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PomodoroApp.Resources
{
    public class TaskModel
    {

        public string taskName { get; set; }
        public int taskTime { get; set; }
        public string taskDescription { get; set; }

        public bool isDone { get; set; }

        public bool inProgres { get; set; }

        public int id { get; set; }
        public bool Important { get; set; }

        public bool Notify { get; set; }
    }
}
