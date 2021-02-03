using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Bemby.AccountModule.Application.Interfaces.Services;
using MailKit.Security;
using MimeKit;

namespace Bemby.AccountModule.Application.Services
{
    public class EmailService : IEmailService
    {
        public bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // ReSharper disable once CA1806
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async Task SendEmail(string serverAddress, int serverPort, string userName, string password, MimeMessage message)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(serverAddress, serverPort, SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                await client.AuthenticateAsync(userName, password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}