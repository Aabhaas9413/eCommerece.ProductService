using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        return services;
    }
}
