using NewPetHome.Applications.Abstraction;

namespace NewPetHome.Applications.SpeciesManagement.Commands.Create;

public record CreateSpecieCommand(string Name) : ICommand;