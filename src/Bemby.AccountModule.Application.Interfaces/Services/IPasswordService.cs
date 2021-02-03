namespace Bemby.AccountModule.Application.Interfaces.Services
{
    public interface IPasswordService
    {
        bool IsStrongPassword(string password);
        string HashPassword(string password, string salt = null, int? numberOfIterations = null);
        bool CheckPassword(string password, string hashResult);
        string GenerateSalt(int saltLength = 32);
    }
}