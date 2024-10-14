using Microsoft.Extensions.DependencyInjection;
using NewPetHome.Species.Contracts;

namespace NewPetHome.Species.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesPresentation(
        this IServiceCollection services)
    {
        services.AddScoped<ISpeciesContract, SpeciesContract>();

        return services;
    }
}