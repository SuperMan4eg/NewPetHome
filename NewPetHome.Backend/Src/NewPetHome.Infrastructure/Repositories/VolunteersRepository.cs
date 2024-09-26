using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NewPetHome.Applications.VolunteersManagement;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Infrastructure.DbContexts;

namespace NewPetHome.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly WriteDbContext _dbContext;

    public VolunteersRepository(WriteDbContext dbContext)
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