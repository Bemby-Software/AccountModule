namespace Bemby.AccountModule.Application.Interfaces.Services
{
    public interface IPhoneService
    {
        bool IsValidPhoneNumber(string mobileNumber);
    }
}