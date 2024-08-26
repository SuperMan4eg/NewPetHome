using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NewPetHome.Applications.Volunteers.CreateVolunteer;

namespace NewPetHome.Applications;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        
        return services;
    }
}