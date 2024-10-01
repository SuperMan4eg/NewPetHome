using NewPetHome.Applications.SpeciesManagement.Commands.AddBreed;

namespace NewPetHome.API.Controllers.Species.Requests;

public record AddBreedRequest(string Name)
{
    public AddBreedCommand ToCommand(Guid specieId) =>
        new(specieId,
            Name);
}