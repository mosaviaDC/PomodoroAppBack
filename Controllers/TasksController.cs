 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PomodoroApp.Models;
using PomodoroApp.Resources;
using Pomodoroapp.Models;
using Microsoft.AspNetCore.Cors;

namespace PomodoroApp.Controllers
{
 
    [Authorize]
    [Route("[controller]")]
    [ApiController]

    public class TasksController : ControllerBase
    {

      
        private readonly IDataRepository repository;
        private User currentUser;
       
        public TasksController (IDataRepository repo)
        {

            repository = repo;

        }

        [HttpGet]
        public async Task<List<Tasks>> GetTasks()
        {


            currentUser = await repository.Users.FirstAsync(u => u.Email == User.Identity.Name);

            return await repository.GetAllUserTasks(currentUser.Id);

        }


        [HttpPost("addtask")]
        public async Task< IActionResult>  AddTask(TaskModel taskModel)
        {
            double taskTime = taskModel.taskTime;

            int taskPeriods = (int)Math.Ceiling(taskTime / 25);

            int lastPeriodTime = 25 - (taskPeriods * 25 - taskModel.taskTime);
            int CurrentPomodoroTime = 25;
            if (taskPeriods == 1)
            {
                CurrentPomodoroTime = lastPeriodTime;
            }


            currentUser = await repository.Users.FirstAsync(u => u.Email == User.Identity.Name);//???возможно есть вариант получше?


            Tasks task = new Tasks()
            {
                TaskName = taskModel.taskName,
                TaskDescription = taskModel.taskDescription,
                TaskPeriods = taskPeriods - 1,
                TaskDateTime = DateTime.Now,
                Notify = false,
                LastPeriodTime = lastPeriodTime,
                CurrentPomodoroTime = CurrentPomodoroTime,
                inProgress = false,
                UserId = currentUser.Id,
                Important = false 
                    


                };


            return CreatedAtAction("addtask", await repository.AddTask(task));




        }

        [HttpPut ("updatestatus")]

        public async Task< IActionResult> UpdateTaskStatus(Tasks taskModel)
        {
         
          
         
            return Ok(await repository.UpdateTaskPartial(taskModel));

         
        }






        [HttpDelete("delete/{id}")] 
        public async Task<IActionResult> DeleteTask (int id){
            return Ok( await repository.DeleteTask(id));
        }
    }
}
