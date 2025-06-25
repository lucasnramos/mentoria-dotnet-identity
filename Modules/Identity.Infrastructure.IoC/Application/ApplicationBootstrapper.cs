using System;
using Identity.Application.AppUser;
using Identity.Application.AppUser.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HttpHandler;
using HttpHandler.Interfaces;

namespace Identity.Infrastructure.IoC.Application;

internal class ApplicationBootstrapper
{
    internal void ChildServiceRegister(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IUserAppService, UserAppService>();
        serviceCollection.AddScoped<IHttpHandler, HttpHandlerBase>();
    }
}
