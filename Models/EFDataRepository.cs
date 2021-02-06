using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PomodoroApp.Models;
using Pomodoroapp.Models;
namespace pomodoroapp.Models
{
    public class EFDataRepository : IDataRepository 
    {
        private readonly EFDataContext context;

       
        public EFDataRepository (EFDataContext _context)
        {
            context = _context;
        }
         
        public List<Tasks> GetAllTasksInProgress()
        {
            return context.Tasks.Where(t => t.inProgress).ToList();
        }

        public async Task<string> UpdateTelegramAccount(string email,int chatId,string telegramUserName)
        {
           var currentUser= await context.Users.FindAsync(email);
            if (currentUser != null)
            {
                currentUser.telegramChatId = chatId;
                currentUser.telegramUserName = telegramUserName;
                await context.SaveChangesAsync();
                return ($"Почта {email} успешно добавлена");

            }
            else
            {
                return ($"Аккаунт {email} не найден");
            }
        }
       
        public async Task< Tasks> GetTask(int id)
        {
            return await context.Tasks.FindAsync(id);
        }

        public async Task<List<Tasks>>  GetAllUserTasks(string userId)
        {

            return await context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }


        public async Task<Tasks> UpdateTaskPartial (Tasks changedTask)
        {
            Tasks originalTask = context.Tasks.Find(changedTask.Id);
            originalTask.inProgress = changedTask.inProgress;
            originalTask.IsDone = changedTask.IsDone;
            originalTask.Important = changedTask.Important;
            originalTask.Notify = changedTask.Notify;
            originalTask.TaskDescription = changedTask.TaskDescription;
            originalTask.TaskDateTime = changedTask.TaskDateTime;

            await context.SaveChangesAsync();
            return (originalTask);
        }
       
        public async  void UpdateTask(Tasks changedTask)
        {
            context.Tasks.Update(changedTask);
            await context.SaveChangesAsync();
        }

        public async Task<Tasks> DeleteTask(int id)
        {
            Tasks removableTask = new Tasks { Id = id };
            context.Tasks.Remove(removableTask);
             await context.SaveChangesAsync();
            return removableTask;
           
        }
        public async Task<Tasks> AddTask(Tasks newTask)
        {
            
            context.Tasks.Add(newTask);
            await context.SaveChangesAsync();
            return newTask;
        }

        public IQueryable<Tasks> Tasks => context.Tasks;

        public IQueryable<User> Users => context.Users;
    }
}
