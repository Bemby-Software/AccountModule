using System.Threading.Tasks;
using Bemby.AccountModule.Application.Interfaces.Repositories;
using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Domain.DTOs;
using Bemby.AccountModule.Domain.Entities;
using MimeKit;
using Nytte.Email;

namespace Bemby.AccountModule.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _accountEmailService;
        private readonly IPhoneService _phoneService;

        public AccountService(IAccountRepository accountRepository, IPasswordService passwordService, IAccountEmailService accountEmailService, IPhoneService phoneService)
        {
            _accountRepository = accountRepository;
            _passwordService = passwordService;
            _accountEmailService = accountEmailService;
            _phoneService = phoneService;
        }
        
        public async Task<AccountEntity> CreateAccountAsync(CreateAccountDto createAccountDto)
        {
            if (!_accountEmailService.IsValidEmailAddress(createAccountDto.Email))
                return null; //ToDo: Throw

            if (!_passwordService.IsStrongPassword(createAccountDto.Password))
                return null; //ToDo: Throw

            if (!_phoneService.IsValidPhoneNumber(createAccountDto.MobileNumber))
                return null; //ToDo: Throw
            
            if (await _accountRepository.DoesAccountExistAsync(createAccountDto.Email))
                return null; //ToDo: Throw

            var accountEntity = new AccountEntity(createAccountDto.Email, createAccountDto.MobileNumber, _passwordService.HashPassword(createAccountDto.Password));
            await _accountRepository.CreateAccountAsync(accountEntity);
            
            return accountEntity;
        }
    }
}