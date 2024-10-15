using Microsoft.Extensions.DependencyInjection;
using NewPetHome.Volunteers.Contracts;
using NewPetHome.Volunteers.Contracts.Converters;
using NewPetHome.Volunteers.Presentation.Processors;

namespace NewPetHome.Volunteers.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersPresentation(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteersContract, VolunteersContract>();
        services.AddScoped<IFormFileConverter, FormFileConverter>();

        return services;
    }
}