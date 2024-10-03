using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Dtos;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Applications.VolunteersManagement.Queries.GetVolunteerById;

public class GetVolunteerByIdHandlerDapper : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetVolunteerByIdHandlerDapper> _logger;

    public GetVolunteerByIdHandlerDapper(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetVolunteerByIdHandlerDapper> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto,ErrorList>> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", query.VolunteerId);

        var sql = """
                  SELECT 
                      id, 
                      first_name,
                      last_name,
                      description,
                      email,
                      experience,
                      phone_number,
                      requisites,
                      social_networks
                  FROM Volunteers WHERE Id = @Id
                  """;

        var volunteers = await connection.QueryAsync<VolunteerDto, string, string, VolunteerDto>(
            sql, (volunteer, requisitesJson, socialNetworkJson) =>
            {
                var requisites = JsonSerializer.Deserialize<RequisiteDto[]>(requisitesJson) ?? [];
                var socialNetworks = JsonSerializer.Deserialize<SocialNetworkDto[]>(socialNetworkJson) ?? [];

                volunteer.Requisites = requisites;
                volunteer.SocialNetworks = socialNetworks;

                return volunteer;
            },
            splitOn: "requisites, social_networks",
            param: parameters);

        var result = volunteers.FirstOrDefault();
        if (result is null)
            return Errors.General.NotFound().ToErrorList();
        
        _logger.LogInformation("Volunteer was received with id: {volunteerId}", result.Id);

        return result;
    }
}