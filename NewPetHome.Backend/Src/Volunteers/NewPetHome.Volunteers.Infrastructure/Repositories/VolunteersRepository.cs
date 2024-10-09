using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NewPetHome.Volunteers.Application;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Volunteers.Domain.Entities;
using NewPetHome.Volunteers.Infrastructure.DbContexts;

namespace NewPetHome.Volunteers.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly VolunteersWriteDbContext _dbContext;

    public VolunteersRepository(VolunteersWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(
        Volunteer volunteer,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteer.AddAsync(volunteer, cancellationToken);

        return volunteer.Id;
    }

    public Guid Save(
        Volunteer volunteer,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteer.Attach(volunteer);

        return volunteer.Id.Value;
    }

    public Guid Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteer.Remove(volunteer);

        return volunteer.Id.Value;
    }

    public async Task<Result<Volunteer, Error>> GetById(
        VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteer
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer is null)
            return Errors.General.NotFound(volunteerId);

        return volunteer;
    }
}