using Microsoft.Extensions.Hosting;
using PomodoroApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using PomodoroApp.TelegramBot;


namespace pomodoroapp.Modules
{
    public class TimerModule : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly Bot telegramBot;

        public  TimerModule (IServiceScopeFactory serviceScopeFactory)
        {
            scopeFactory = serviceScopeFactory;
            telegramBot = new Bot(scopeFactory);
   
         
        
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            timer = new Timer(Pomodoro, null, 0,60000);

            return Task.CompletedTask;

        }
        private async void Pomodoro(object state)
        {

            //Список всех пользователей
            




            using (var scope = scopeFactory.CreateScope())
            {
                EFDataContext userContext = scope.ServiceProvider.GetRequiredService<EFDataContext>();


                List<User> users = new List<User>();


                var tasks = await userContext.Tasks.ToListAsync();
               
                foreach (var task in tasks)
                {
                  users.AddRange(await   userContext.Users.Where(u => u.Id == task.UserId).Where(u=>u.telegramChatId!=0).ToListAsync());
                  users = users.Distinct().ToList();
                  


                    if (task.Notify)
                    {
                        if (task.TaskDateTime.Minute == DateTime.UtcNow.Minute && task.TaskDateTime.Hour == DateTime.UtcNow.Hour && task.TaskDateTime.Day == DateTime.UtcNow.Day)
                        {
                            if(!task.Important)
                            telegramBot.SendMessage(users.First(u => u.Id == task.UserId).telegramChatId, $"⚡ Напонимаю о задаче: {task.TaskName} ⚡");
                            else
                            {
                                telegramBot.SendMessage(users.First(u => u.Id == task.UserId).telegramChatId, $"⚡ Напонимаю о задаче: {task.TaskName} ⚡\n" +
                                    $"⭐Эта задача отмечена как важная ⭐");
                            }
                        }
                    }


                    if (task.inProgress && task.CurrentPomodoroTime >=0 && !task.InPomodoroPause && task.TaskPeriods >=0)
                    {
                        //Console.WriteLine("Текущий период:" + task.TaskPeriods +  "время: " + task.CurrentPomodoroTime);
                        task.CurrentPomodoroTime=task.CurrentPomodoroTime - 1;
                       

                        if (task.CurrentPomodoroTime == 0)
                        {
                            //Конец периода
                            if (task.TaskPeriods >= 0)
                            {

                                task.TaskPeriods = task.TaskPeriods - 1;
                                task.InPomodoroPause = true; 
                                task.CurrentPomodoroPauseTime = 5;




                                if (task.TaskPeriods >=0)
                                {
                                    var user = userContext.Users.First(u => u.Id == task.UserId);

                                    if (user.telegramChatId != 0)
                                    {

                                        string respone = ($"Для задачи: {task.TaskName} пора сделать перерыв 🌴");

                                        telegramBot.SendMessage(user.telegramChatId, respone);
                                    }
                                }
                                if (task.TaskPeriods == -1)
                                {
                                    var user = userContext.Users.First(u => u.Id == task.UserId);

                                    if (user.telegramChatId != 0)
                                    {

                                        string respone = ($"Задача: {task.TaskName}, завершена");

                                        telegramBot.SendMessage(user.telegramChatId, respone);
                                    }

                                }
                                
                                if(task.TaskPeriods == -1)
                                {
                                    task.inProgress = false;
                                    task.IsDone = true;
                                }
                            }
                           
                        }

                    }
                    else if (task.InPomodoroPause && task.CurrentPomodoroPauseTime >=0 && task.inProgress) 
                    {

                        if (task.TaskPeriods == -1)
                        {
                            //Конец
                           
                        }
                        else if  (task.CurrentPomodoroPauseTime >= 0)
                        {
                            //Console.WriteLine("Перерыв " + task.CurrentPomodoroPauseTime);
                                 task.CurrentPomodoroPauseTime = task.CurrentPomodoroPauseTime - 1;
                                 if (task.CurrentPomodoroPauseTime == 0)
                                {

                                var user = userContext.Users.First(u => u.Id == task.UserId);

                                if (user.telegramChatId != 0)
                                {

                                    string respone = ($"У задачи: {task.TaskName} перерыв закончился 🕴️");

                                    telegramBot.SendMessage(user.telegramChatId, respone);
                                }


                                if (task.TaskPeriods == 0)
                                        {

                                            task.InPomodoroPause = false;
                                            task.CurrentPomodoroTime = task.LastPeriodTime;
                                        }
                                        else
                                        {
                                            
                                            task.InPomodoroPause = false;
                                            task.CurrentPomodoroTime = 25;
                                        }

                                }     

                        }
                     }
                            
                       
                }

             
                await userContext.SaveChangesAsync();

                if (DateTime.UtcNow.Minute == 10)
                {


                    foreach (var user in users)
                    {
                        string taskNames = "";

                        List<Tasks> currentDayUserTasks = new List<Tasks>();
                        currentDayUserTasks.AddRange(tasks.Where(t => t.UserId == user.Id));
                        currentDayUserTasks.Distinct();
                        int i = 0;
                        foreach (var task in currentDayUserTasks)
                        {
                            if (!task.IsDone && task.TaskDateTime.Day == DateTime.UtcNow.Day)
                            {
                                i++;
                                taskNames += $"{i}.  {task.TaskName}\n";
                            }


                        }
                        telegramBot.SendMessage(user.telegramChatId, $"Cписок задач на сегодня:\n{taskNames}Хорошего дня!");


                    }


                }


            }

           




        }

        public Task StopAsync (CancellationToken stoppingToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    