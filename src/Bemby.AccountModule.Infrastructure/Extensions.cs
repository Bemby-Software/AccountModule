using System;
using Bemby.AccountModule.Infrastructure.Documents;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.Extensions.DependencyInjection;

namespace Bemby.AccountModule.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddAccountModuleInfrastructure(this IServiceCollection services)
        {
            services.AddConvey().AddMongo().AddMongoRepository<AccountDocument, Guid>("accounts");
            return services;
        }
    }
}