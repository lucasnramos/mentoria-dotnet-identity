using Identity.Infrastructure.IoC.Application;
using Identity.Infrastructure.IoC.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.IoC;

public class RootBootstrapper
{
    public void BootstrapperRegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        new RepositoryBootstrapper().ChildServiceRegister(services, configuration);
        new ApplicationBootstrapper().ChildServiceRegister(services, configuration);
    }
}
