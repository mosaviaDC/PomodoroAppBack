using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

namespace pomodoroapp
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, BodyBuilder bodyBuilder)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("PomodoroTracker.ru", "admin@pomodorotracker.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.beget.com", 465, true);
                await client.AuthenticateAsync("admin@pomodorotracker.ru", "{3657Lok}eB8mnR");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
