using ApproveMe.Core.Helpers;
using ApproveMe.Core.Interfaces;
using ApproveMe.Core.Services;
using ApproveMe.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApproveMe.Core;

public static class ServiceRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IRoleService, RoleService>();
    }

    public static void RegisterHelpers(this IServiceCollection services)
    {
        services.AddScoped<AuthHelper>();
        services.AddScoped<UserHelper>();
        services.AddScoped<UserSessionHelper>();
        services.AddScoped<RoleHelper>();
    }
    
    public static void AddDbContext(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }
}