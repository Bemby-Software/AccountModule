using System;
using System.Threading.Tasks;
using Bemby.AccountModule.Domain.Entities;

namespace Bemby.AccountModule.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task CreateAccountAsync(IAccountEntity accountEntity);
        Task<IAccountEntity> GetAccountAsync(Guid accountId);
        Task<IAccountEntity> GetAccountAsync(string email);
        Task<bool> DoesAccountExistAsync(Guid accountId);
        Task<bool> DoesAccountExistAsync(string email);
        Task UpdateAccountAsync(IAccountEntity updatedAccountEntity);
    }
}