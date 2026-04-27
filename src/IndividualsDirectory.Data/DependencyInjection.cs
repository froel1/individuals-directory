using IndividualsDirectory.Data.Context;
using IndividualsDirectory.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IndividualsDirectory.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IIndividualRepository, IndividualRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IIndividualConnectionRepository, IndividualConnectionRepository>();
        services.AddScoped<UnitOfWork.IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
