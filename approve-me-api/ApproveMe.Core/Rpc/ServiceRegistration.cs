using ApproveMe.Core.Rpc.Interfaces;
using ApproveMe.Core.Rpc.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApproveMe.Core.Rpc;

public static class ServiceRegistration
{
    public static void RegisterRpc(this IServiceCollection services)
    {
        services.AddScoped<IPredictionService, PredictionService>();
    }
}