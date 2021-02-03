using System.Threading.Tasks;
using Bemby.AccountModule.Domain.DTOs;
using Bemby.AccountModule.Domain.Entities;

namespace Bemby.AccountModule.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<AccountEntity> CreateAccountAsync(CreateAccountDto createAccountDto);
    }
}