using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;
using NewPetHome.Core.Extensions;
using NewPetHome.Core.Models;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Queries.GetVolunteersWithPagination;

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

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
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