using IndividualsDirectory.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IndividualsDirectory.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
