using Identity.Infrastructure.IoC;
using Authentication.Adapter.Configurations;
using Authentication.Adapter.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
        options.AddPolicy("CorsPolicy",
                builder =>
                builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

#region JWT
// Token and JWT service with Authentication Adapter
var tokenConfigurations = new TokenConfigurations();
new ConfigureFromConfigurationOptions<TokenConfigurations>(builder.Configuration.GetSection("TokenConfigurations"))
        .Configure(tokenConfigurations);

builder.Services.AddJwtSecurity(tokenConfigurations);
#endregion

new RootBootstrapper().BootstrapperRegisterServices(builder.Services, builder.Configuration);
var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();