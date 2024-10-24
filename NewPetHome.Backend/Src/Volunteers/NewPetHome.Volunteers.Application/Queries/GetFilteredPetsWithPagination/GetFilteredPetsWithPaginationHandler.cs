using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;
using NewPetHome.Core.Extensions;
using NewPetHome.Core.Models;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Queries.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly ILogger<GetFilteredPetsWithPaginationHandler> _logger;

    public GetFilteredPetsWithPaginationHandler(
        IVolunteersReadDbContext volunteersReadDbContext,
        ILogger<GetFilteredPetsWithPaginationHandler> logger)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
        _logger = logger;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var keySelector = SortByProperty(query.SortBy);

        var petsQuery = ApplyFilters(_volunteersReadDbContext.Pets, query);
        
        petsQuery = query.SortDirection?.ToLower() == "desc"
            ? petsQuery.OrderByDescending(keySelector)
            : petsQuery.OrderBy(keySelector);

        var result = await petsQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);

        _logger.LogInformation("Pets was received with count: {totalCount}", result.Items.Count);

        return result;
    }

    private static Expression<Func<PetDto, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return v => v.Id;

        Expression<Func<PetDto, object>> keySelector = sortBy?.ToLower() switch
        {
            "volunteer" => p => p.VolunteerId,
            "species" => p => p.SpeciesId,
            "breed" => p => p.BreedId,
            "name" => p => p.Name,
            "color" => p => p.Color,
            "status" => p => p.Status,
            "city" => p => p.City,
            "street" => p => p.Street,
            "age" => p => p.BirthDate,
            "weight" => p => p.Weight,
            "height" => p => p.Height,
            _ => p => p.Id
        };

        return keySelector;
    }

    private static IQueryable<PetDto> ApplyFilters(
        IQueryable<PetDto> petsQuery,
        GetFilteredPetsWithPaginationQuery query)
    {
        return petsQuery
            .WhereIf(!string.IsNullOrWhiteSpace(query.Name), p => p.Name.Contains(query.Name!))
            .WhereIf(!string.IsNullOrWhiteSpace(query.Color), p => p.Color.Contains(query.Color!))
            .WhereIf(!string.IsNullOrWhiteSpace(query.Status), p => p.Status == query.Status)
            .WhereIf(!string.IsNullOrWhiteSpace(query.City), p => p.City.Contains(query.City!))
            .WhereIf(!string.IsNullOrWhiteSpace(query.Street), p => p.Street.Contains(query.Street!))
            .WhereIf(query.AgeTo != null, p => (DateTime.Now - p.BirthDate).TotalDays / 365 <= query.AgeTo)
            .WhereIf(query.AgeFrom != null, p => (DateTime.Now - p.BirthDate).TotalDays / 365 >= query.AgeFrom)
            .WhereIf(query.WeightTo != null, p => p.Weight <= query.WeightTo)
            .WhereIf(query.WeightFrom != null, p => p.Weight >= query.WeightFrom)
            .WhereIf(query.HeightTo != null, p => p.Height <= query.HeightTo)
            .WhereIf(query.HeightFrom != null, p => p.Height >= query.HeightFrom)
            .WhereIf(query.VolunteerId.HasValue, p => p.VolunteerId == query.VolunteerId)
            .WhereIf(query.SpeciesId.HasValue, p => p.SpeciesId == query.SpeciesId)
            .WhereIf(query.BreedId.HasValue, p => p.BreedId == query.BreedId)
            .WhereIf(query.IsCastrated.HasValue, p => p.IsCastrated == query.IsCastrated)
            .WhereIf(query.IsVaccinated.HasValue, p => p.IsVaccinated == query.IsVaccinated);
    }
} 