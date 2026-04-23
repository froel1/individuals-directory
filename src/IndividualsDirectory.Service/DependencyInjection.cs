using IndividualsDirectory.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IndividualsDirectory.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IIndividualService, IndividualService>();
        return services;
    }
}
