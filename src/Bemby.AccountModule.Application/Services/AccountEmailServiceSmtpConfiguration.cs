using Bemby.AccountModule.Application.Interfaces.Services;
using Nytte.Email;

namespace Bemby.AccountModule.Application.Services
{
    public class AccountEmailServiceSmtpConfiguration : EmailServiceSmtpServerConfiguration, IAccountEmailServiceSmtpConfiguration
    {
        public AccountEmailServiceSmtpConfiguration() : base("smtp.office365.com", 587, "robertbennett1998@outlook.com", "serverPassword")
        {
        }
    }
}