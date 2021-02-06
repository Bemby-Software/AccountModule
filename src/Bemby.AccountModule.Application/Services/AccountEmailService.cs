using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Bemby.AccountModule.Application.Interfaces.Services;
using MailKit.Security;
using MimeKit;
using Nytte.Email;

namespace Bemby.AccountModule.Application.Services
{
    public class AccountEmailService : EmailService, IAccountEmailService
    {
        public AccountEmailService(IAccountEmailServiceSmtpConfiguration serviceSmtpServerConfiguration) : base(serviceSmtpServerConfiguration)
        {
        }
    }
}