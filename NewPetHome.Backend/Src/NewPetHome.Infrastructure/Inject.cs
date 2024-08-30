using Microsoft.Extensions.DependencyInjection;
using NewPetHome.Applications.Volunteers;
using NewPetHome.Infrastructure.Repositories;

namespace NewPetHome.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();

        services.AddScoped<IVolunteersRepository, VolunteersRepository>();

        return services;
    }
}