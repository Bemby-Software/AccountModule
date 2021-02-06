using System.Reflection;
using Bemby.AccountModule.Application.Interfaces.Repositories;
using Bemby.AccountModule.Application.Interfaces.Services;
using Bemby.AccountModule.Application.Services;
using Bemby.AccountModule.Infrastructure;
using Bemby.AccountModule.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nytte.Email;

namespace Bemby.AccountModule.Api
{
    public static class Extensions
    {
        public static IServiceCollection AddAccountModule(this IServiceCollection services)
        {
            services.AddControllers().AddApplicationPart(Assembly.GetExecutingAssembly()).AddControllersAsServices();
            
            services.AddAccountModuleInfrastructure();
            
            services.AddSingleton<IPasswordService, PBKDF2PasswordService>();
            services.AddSingleton<IAccountEmailServiceSmtpConfiguration, AccountEmailServiceSmtpConfiguration>();
            services.AddSingleton<IAccountEmailService, AccountEmailService>();
            services.AddSingleton<IPhoneService, PhoneService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            
            return services;
        }
        
        public static IApplicationBuilder UseAccountModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}