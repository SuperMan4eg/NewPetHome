using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewPetHome.Core.Abstraction;
using NewPetHome.Core.Dtos;
using NewPetHome.SharedKernel;

namespace NewPetHome.Volunteers.Application.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;
    private readonly ILogger<GetPetByIdHandler> _logger;

    public GetPetByIdHandler(
        IVolunteersReadDbContext volunteersReadDbContext,
        ILogger<GetPetByIdHandler> logger)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
        _logger = logger;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var petResult = await _volunteersReadDbContext.Pets
            .FirstOrDefaultAsync(p => p.Id == query.PetId, cancellationToken);
        if (petResult is null)
            return Errors.General.NotFound().ToErrorList();

        _logger.LogInformation("Pet was received with id: {petId}", query.PetId);

        return petResult;
    }
}