using System.Text.Json.Serialization;

namespace Bemby.AccountModule.Domain.DTOs
{
    public class CreateAccountDto
    {
        public string Email { get; }
        public string MobileNumber { get; }
        public string Password { get; }

        [JsonConstructor]
        public CreateAccountDto(string email, string mobileNumber, string password)
        {
            Email = email;
            MobileNumber = mobileNumber;
            Password = password;
        }
    }
}