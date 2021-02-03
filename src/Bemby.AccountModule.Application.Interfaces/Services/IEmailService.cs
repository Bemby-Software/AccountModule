using System.Threading.Tasks;
using MimeKit;

namespace Bemby.AccountModule.Application.Interfaces.Services
{
    public interface IEmailService
    {
        bool IsValidEmailAddress(string email);
        Task SendEmail(string serverAddress, int serverPort, string userName, string password, MimeMessage message);
    }
}