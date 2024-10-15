using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;
using NewPetHome.Core.Extensions;
using NewPetHome.Core.Models;
using NewPetHome.SharedKernel;

namespace NewPetHome.Species.Applications.Queries.GetAllSpeciesWithPagination;

public class GetAllSpecieWithPaginationHandler : IQueryHandler<PagedList<SpecieDto>, GetAllSpeciesWithPaginationQuery>
{
    private readonly ISpeciesReadDbContext _speciesReadDbContext;
    private readonly ILogger<GetAllSpecieWithPaginationHandler> _logger;

    public GetAllSpecieWithPaginationHandler(
        ISpeciesReadDbContext speciesReadDbContext,
        ILogger<GetAllSpecieWithPaginationHandler> logger)
    {
        _speciesReadDbContext = speciesReadDbContext;
        _logger = logger;
    }

    public async Task<Result<PagedList<SpecieDto>, ErrorList>> Handle(
        GetAllSpeciesWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var speciesQuery = _speciesReadDbContext.Species;

        speciesQuery = query.SortDirection?.ToLower() == "desc"
            ? speciesQuery.OrderByDescending(keySelector => keySelector.Name)
            : speciesQuery.OrderBy(keySelector => keySelector.Name);

        var result = await speciesQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);

        _logger.LogInformation("Species was received with count: {totalCount}", result.Items.Count);

        return result;
    }
}