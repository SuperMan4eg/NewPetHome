using NewPetHome.Applications.Abstraction;

namespace NewPetHome.Applications.VolunteersManagement.Commands.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;