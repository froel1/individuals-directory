using IndividualsDirectory.Service.Images;
using Microsoft.Extensions.DependencyInjection;

namespace IndividualsDirectory.Service;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddSingleton<IImageStorageService, FileSystemImageStorageService>();
        return services;
    }
}
