using NewPetHome.Core.Abstraction;

namespace NewPetHome.Species.Applications.Commands.AddBreed;

public record AddBreedCommand(Guid SpecieId, string Name) : ICommand;