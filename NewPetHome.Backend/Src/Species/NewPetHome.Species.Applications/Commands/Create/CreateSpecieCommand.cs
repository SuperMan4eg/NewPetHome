using NewPetHome.Core.Abstraction;

namespace NewPetHome.Species.Applications.Commands.Create;

public record CreateSpecieCommand(string Name) : ICommand;