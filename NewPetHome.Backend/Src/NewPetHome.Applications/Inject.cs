using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NewPetHome.Applications.Volunteers.AddPet;
using NewPetHome.Applications.Volunteers.Create;
using NewPetHome.Applications.Volunteers.Delete;
using NewPetHome.Applications.Volunteers.UpdateMainInfo;
using NewPetHome.Applications.Volunteers.UpdateRequisites;
using NewPetHome.Applications.Volunteers.UpdateSocialNetworks;

namespace NewPetHome.Applications;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateRequisitesHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddScoped<AddPetHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}