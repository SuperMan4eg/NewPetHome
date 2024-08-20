using Microsoft.EntityFrameworkCore;
using NewPetHome.Applications.Volunteers;
using NewPetHome.Domain.Volunteers;

namespace NewPetHome.Infrastructure.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public VolunteersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteer.AddAsync(volunteer, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return volunteer.Id;
    }

    public async Task<Volunteer> GetById(VolunteerId volunteerId, CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteer
            .Include(v=>v.Pets)
            .FirstOrDefaultAsync(v=>v.Id==volunteerId, cancellationToken);
        
        if(volunteer is null)
            throw new Exception("Volunteer not found");
        
        return volunteer;
    }
}