using NewPetHome.Core.Abstraction;

namespace NewPetHome.Species.Applications.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpecieId, Guid BreedId) : ICommand;