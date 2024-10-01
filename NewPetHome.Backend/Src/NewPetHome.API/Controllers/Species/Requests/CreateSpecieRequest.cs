using NewPetHome.Applications.SpeciesManagement.Commands.Create;

namespace NewPetHome.API.Controllers.Species.Requests;

public record CreateSpecieRequest(string Name)
{
    public CreateSpecieCommand ToCommand() =>
        new(Name);
}