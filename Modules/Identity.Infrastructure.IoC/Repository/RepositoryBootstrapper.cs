using System;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Repositories.Repository;
using Marraia.MongoDb.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.IoC.Repository;

internal class RepositoryBootstrapper
{
    internal void ChildServiceRegister(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddMongoDb();
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
    }
}
