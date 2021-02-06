using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PomodoroApp.Models;
namespace Pomodoroapp.Models
{
     public interface IDataRepository
    {


        IQueryable <Tasks> Tasks { get; }
        IQueryable <User> Users { get; }
        /// <summary>
        /// Получение задачи по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Tasks> GetTask(int id);

        /// <summary>
        /// Обновление задачи (только некоторых свойств, см EFDataContext)
        /// </summary>
        /// <param name="task"></param>
        Task<Tasks> UpdateTaskPartial(Tasks changedTask);

        /// <summary>
        /// Получение всех задач находящихся в работе
        /// </summary>
        /// <returns></returns>
        List<Tasks> GetAllTasksInProgress();

        /// <summary>
        /// Получение всех задач пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task< List<Tasks>> GetAllUserTasks(string userId);
       
        /// <summary>
        /// Обновление всех свойств
        /// </summary>
        /// <param name="changedTask"></param>
        void UpdateTask(Tasks changedTask);
        /// <summary>
        /// Добавление задачи
        /// </summary>
        /// <param name="newTask"></param>
        Task<Tasks> AddTask(Tasks newTask);


        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="id"></param>
        Task<Tasks> DeleteTask(int id);

        /// <summary>
        /// Добавление к аккаунту телеграмм уведомлений
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Строка ответа пользователю</returns>
         Task<string> UpdateTelegramAccount(string email, int chatId, string telegramUserName);


    }
}
