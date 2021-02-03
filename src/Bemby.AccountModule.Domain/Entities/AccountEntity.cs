using System;

namespace Bemby.AccountModule.Domain.Entities
{
    public class AccountEntity : IAccountEntity
    {
        public AccountEntity(string email, string mobileNumber, string hashedPassword)
        {
            Id = Guid.NewGuid();
            Email = email;
            MobileNumber = mobileNumber;
            HashedPassword = hashedPassword;
        }
        
        public AccountEntity(Guid id, string email, string mobileNumber, string hashedPassword)
        {
            Id = id;
            Email = email;
            MobileNumber = mobileNumber;
            HashedPassword = hashedPassword;
        }
        
        public Guid Id { get; set; }
        public string Email { get; }
        public string MobileNumber { get; }
        public string HashedPassword { get; }
    }
}