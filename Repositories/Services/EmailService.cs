using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using TestToken.Data;
using TestToken.Models;
using TestToken.Repositories.GenericRepository;
using TestToken.Repositories.Interfaces;
using TestToken.UOW;
using MailKit.Net.Smtp;

namespace TestToken.Repositories.Services
{
    public class EmailService : IEmailService
    {
       
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> options) 
        {
            _emailSettings = options.Value;
        }

        public async Task sendEmailAsync(string Email, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_emailSettings.Email));
            mail.To.Add(MailboxAddress.Parse(Email));
            mail.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = message
            };
            mail.Body = builder.ToMessageBody();
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Email,_emailSettings.Password);
            await smtp.SendAsync(mail);
            await smtp.DisconnectAsync(true);
        }
    }
}
