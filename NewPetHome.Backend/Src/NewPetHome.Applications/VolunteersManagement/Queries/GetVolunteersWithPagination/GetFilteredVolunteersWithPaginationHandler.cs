using Microsoft.Extensions.Logging;
using NewPetHome.Applications.Abstraction;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.Extensions;
using NewPetHome.Applications.Models;

namespace NewPetHome.Applications.VolunteersManagement.Queries.GetVolunteersWithPagination;

public class GetFilteredVolunteersWithPaginationHandler
    : IQueryHandler<PagedList<VolunteerDto>, GetFilteredVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetFilteredVolunteersWithPaginationHandler> _logger;

    public GetFilteredVolunteersWithPaginationHandler(
        IReadDbContext readDbContext,
        ILogger<GetFilteredVolunteersWithPaginationHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<PagedList<VolunteerDto>> Handle(
        GetFilteredVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Volunteers;

        volunteersQuery = volunteersQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.FirstName),
            v => v.FirstName.Contains(query.FirstName!));

        volunteersQuery = volunteersQuery.WhereIf(
            query.ExperienceTo != null,
            v => v.Experience <= query.ExperienceTo);

        volunteersQuery = volunteersQuery.WhereIf(
            query.ExperienceFrom != null,
            v => v.Experience >= query.ExperienceFrom);

        var result = await volunteersQuery
            .OrderBy(v => v.Experience)
            .ToPagedList(query.Page, query.PageSize, cancellationToken);

        _logger.LogInformation("Volunteers was received with count: {totalCount}", result.Items.Count);

        return result;
    }
}