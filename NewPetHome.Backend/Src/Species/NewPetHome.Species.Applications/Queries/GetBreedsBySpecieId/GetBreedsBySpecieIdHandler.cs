using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;
using NewPetHome.Core.Extensions;
using NewPetHome.Core.Models;
using NewPetHome.SharedKernel;

namespace NewPetHome.Species.Applications.Queries.GetBreedsBySpecieId;

public class GetBreedsBySpecieIdHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpecieIdQuery>
{
    private readonly ISpeciesReadDbContext _speciesReadDbContext;
    private readonly ILogger<GetBreedsBySpecieIdHandler> _logger;

    public GetBreedsBySpecieIdHandler(
        ISpeciesReadDbContext speciesReadDbContext,
        ILogger<GetBreedsBySpecieIdHandler> logger)
    {
        _speciesReadDbContext = speciesReadDbContext;
        _logger = logger;
    }

    public async Task<Result<PagedList<BreedDto>, ErrorList>> Handle(
        GetBreedsBySpecieIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var breedsQuery = _speciesReadDbContext.Breeds;

        breedsQuery = breedsQuery.Where(b => b.SpecieId == query.SpecieId);

        var result = await breedsQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);

        _logger.LogInformation("Breeds was received with count: {totalCount}", result.Items.Count);
        
        return result;
    }
}