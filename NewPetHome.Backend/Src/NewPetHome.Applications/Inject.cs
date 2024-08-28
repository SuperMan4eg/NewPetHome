using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NewPetHome.Applications.Volunteers.Create;
using NewPetHome.Applications.Volunteers.UpdateMainInfo;

namespace NewPetHome.Applications;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}