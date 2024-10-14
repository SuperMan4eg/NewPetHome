using NewPetHome.Core.Abstraction;

namespace NewPetHome.Species.Applications.Commands.Delete;

public record DeleteSpecieCommand(Guid SpecieId) : ICommand;