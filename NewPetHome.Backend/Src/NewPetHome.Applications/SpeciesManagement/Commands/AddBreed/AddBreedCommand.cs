using NewPetHome.Applications.Abstraction;

namespace NewPetHome.Applications.SpeciesManagement.Commands.AddBreed;

public record AddBreedCommand(Guid SpecieId, string Name) : ICommand;