using System.Threading.Tasks;
using Bemby.AccountModule.Application.Interfaces.Repositories;
using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Domain.DTOs;
using Bemby.AccountModule.Domain.Entities;
using MimeKit;

namespace Bemby.AccountModule.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly IPhoneService _phoneService;

        public AccountService(IAccountRepository accountRepository, IPasswordService passwordService, IEmailService emailService, IPhoneService phoneService)
        {
            _accountRepository = accountRepository;
            _passwordService = passwordService;
            _emailService = emailService;
            _phoneService = phoneService;
        }
        
        public async Task<AccountEntity> CreateAccountAsync(CreateAccountDto createAccountDto)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Robert Bennett", "robertbennett1998@outlook.com"));
            message.To.Add(new MailboxAddress("Robert Bennett", "robertbennett1998@gmail.com"));
            message.Subject = "Test";
            
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "test";

            message.Body = bodyBuilder.ToMessageBody();
            await _emailService.SendEmail("smtp.office365.com", 587, "robertbennett1998@outlook.com", "incorrect_password", message);

            if (!_emailService.IsValidEmailAddress(createAccountDto.Email))
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