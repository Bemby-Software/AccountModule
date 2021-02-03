using System.Threading.Tasks;
using Bemby.AccountModule.Api.DTOs;
using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Bemby.AccountModule.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createAccountDto)
        {
            return Ok(AccountCreatedDto.FromEntity(await _accountService.CreateAccountAsync(createAccountDto)));
        }
    }
}