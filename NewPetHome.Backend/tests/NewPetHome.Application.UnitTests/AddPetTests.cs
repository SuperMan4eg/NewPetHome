using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.VolunteersManagement;
using NewPetHome.Applications.VolunteersManagement.Commands.AddPet;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Application.UnitTests;

public class AddPetTests
{
    private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<AddPetCommand>> _validatorMock;
    private readonly Mock<ILogger<AddPetHandler>> _loggerMock;
    private readonly AddPetHandler _handler;
    private readonly CancellationToken _ct;

    public AddPetTests()
    {
        _volunteersRepositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<AddPetCommand>>();
        _loggerMock = new Mock<ILogger<AddPetHandler>>();

        _handler = new AddPetHandler(
            _volunteersRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        _ct = new CancellationTokenSource().Token;
    }

    [Fact]
    public async Task Handle_ShouldAddPet()
    {
        // arrange
        var validationResult = new ValidationResult();

        var volunteer = CreateVolunteerWithPets(0);

        var command = new AddPetCommand(
            volunteer.Id,
            "test",
            "test",
            "test",
            "test",
            new AddressDto("test", "test", 1),
            1,
            1,
            "9999999999",
            false,
            DateTime.Now,
            false,
            "FindHome",
            new List<RequisiteDto>());

        _validatorMock
            .Setup(v => v.ValidateAsync(command, _ct))
            .ReturnsAsync(validationResult);

        _volunteersRepositoryMock
            .Setup(v => v.GetById(volunteer.Id, _ct))
            .ReturnsAsync(volunteer);

        _unitOfWorkMock
            .Setup(v => v.SaveChanges(_ct))
            .Returns(Task.CompletedTask);

        // act
        var result = await _handler.Handle(command, _ct);

        // assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(volunteer.Pets[0].Id.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError()
    {
        // arrange
        var volunteer = CreateVolunteerWithPets(0);

        var command = new AddPetCommand(
            volunteer.Id,
            "test",
            "test",
            "test",
            "test",
            new AddressDto("test", "test", 1),
            1,
            1,
            "9999999999",
            false,
            DateTime.Now,
            false,
            "IDontKnow",
            new List<RequisiteDto>());

        var errors = new List<ValidationFailure>()
        {
            new(nameof(command.Status), Errors.General.ValueIsInvalid(nameof(command.Status)).Serialize())
        };

        var validationResult = new ValidationResult(errors);

        _validatorMock
            .Setup(v => v.ValidateAsync(command, _ct))
            .ReturnsAsync(validationResult);

        // act
        var result = await _handler.Handle(command, _ct);

        // assert
        result.IsFailure.Should().BeTrue();

        var error = result.Error.First();
        error.Type.Should().Be(ErrorType.Validation);
        error.Code.Should().Be(Errors.General.ValueIsInvalid().Code);
        error.InvalidField.Should().Be(nameof(command.Status));
    }

    [Fact]
    public async Task Handle_WhenVolunteerDoesNotExist_ShouldReturnError()
    {
        // arrange
        var validationResult = new ValidationResult();

        var volunteerId = VolunteerId.NewVolunteerId();

        var command = new AddPetCommand(
            volunteerId,
            "test",
            "test",
            "test",
            "test",
            new AddressDto("test", "test", 1),
            1,
            1,
            "9999999999",
            false,
            DateTime.Now,
            false,
            "FindHome",
            new List<RequisiteDto>());

        _volunteersRepositoryMock
            .Setup(v => v.GetById(volunteerId, _ct))
            .ReturnsAsync(Errors.General.NotFound(volunteerId));

        _validatorMock
            .Setup(v => v.ValidateAsync(command, _ct))
            .ReturnsAsync(validationResult);

        // act
        var result = await _handler.Handle(command, _ct);

        // assert
        result.IsFailure.Should().BeTrue();

        var error = result.Error.First();
        error.Type.Should().Be(ErrorType.NotFound);
        error.Code.Should().Be(Errors.General.NotFound().Code);
    }

    private Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var volunteerId = VolunteerId.NewVolunteerId();
        var fullName = FullName.Create("test", "test").Value;
        var description = Description.Create("test").Value;
        var email = Email.Create("test@test.test").Value;
        var experience = Experience.Create(1).Value;
        var phoneNumber = PhoneNumber.Create("9999999999").Value;
        var requisites = new ValueObjectList<Requisite>(new List<Requisite>());
        var socialNetworks = new ValueObjectList<SocialNetwork>(new List<SocialNetwork>());

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            email,
            experience,
            phoneNumber,
            requisites,
            socialNetworks);

        var name = Name.Create("test").Value;
        var speciesId = SpecieId.NewSpeciesId();
        var breedId = BreedId.NewBreedId();
        var typeDetails = TypeDetails.Create(speciesId, breedId).Value;
        var color = Color.Create("test").Value;
        var health = HealthInfo.Create("test").Value;
        var address = Address.Create("test", "test", 1).Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var isCastrated = true;
        var birthDate = DateTime.Now;
        var isVaccinated = true;
        var status = PetStatus.FindHome;
        var photos = new ValueObjectList<Photo>(new List<Photo>());

        for (int i = 0; i < petsCount; i++)
        {
            var pet = new Pet(
                PetId.NewPetId(),
                name,
                description,
                typeDetails,
                color,
                health,
                address,
                weight,
                height,
                phoneNumber,
                isCastrated,
                birthDate,
                isVaccinated,
                status,
                photos,
                requisites);

            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}