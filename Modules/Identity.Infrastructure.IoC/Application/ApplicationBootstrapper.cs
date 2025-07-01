using System;
using Identity.Application.AppUser;
using Identity.Application.AppUser.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.IoC.Application;

internal class ApplicationBootstrapper
{
    internal void ChildServiceRegister(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollection.AddScoped<IUserAppService, UserAppService>();
    }
}
