using IndividualsDirectory.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IndividualsDirectory.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services)
    {
        services.AddSingleton<IIndividualRepository, InMemoryIndividualRepository>();
        return services;
    }
}
