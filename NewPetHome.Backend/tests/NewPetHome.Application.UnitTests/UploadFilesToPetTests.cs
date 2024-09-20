using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using NewPetHome.Applications.Database;
using NewPetHome.Applications.Dtos;
using NewPetHome.Applications.FileProvider;
using NewPetHome.Applications.Volunteers;
using NewPetHome.Applications.Volunteers.UploadFilesToPet;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Application.UnitTests;

public class UploadFilesToPetTests
{
    private readonly Mock<IFileProvider> _fileProviderMock;
    private readonly Mock<IVolunteersRepository> _volunteersRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<UploadFilesToPetCommand>> _validatorMock;
    private readonly Mock<ILogger<UploadFilesToPetHandler>> _loggerMock;
    private readonly UploadFilesToPetHandler _handler;
    private readonly CancellationToken _ct;

    public UploadFilesToPetTests()
    {
        _fileProviderMock = new Mock<IFileProvider>();
        _volunteersRepositoryMock = new Mock<IVolunteersRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<UploadFilesToPetCommand>>();
        _loggerMock = new Mock<ILogger<UploadFilesToPetHandler>>();

        _handler = new UploadFilesToPetHandler(
            _fileProviderMock.Object,
            _volunteersRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        _ct = new CancellationTokenSource().Token;
    }

    [Fact]
    public async Task Handle_ShouldUploadFilesToPet()
    {
        // arrange
        var validationResult = new ValidationResult();

        var volunteer = CreateVolunteerWithPets(1);

        var pet = volunteer.Pets[0];

        var files = CreateUploadFileDtos(2);

        var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, files);

        List<FilePath> filePaths = [];
        filePaths.AddRange(files.Select(file => FilePath.Create(file.FileName).Value));

        _fileProviderMock
            .Setup(f => f.UploadFiles(It.IsAny<List<FileData>>(), _ct))
            .ReturnsAsync(Result.Success<IReadOnlyList<FilePath>, Error>(filePaths));

        _volunteersRepositoryMock
            .Setup(v => v.GetById(volunteer.Id, _ct))
            .ReturnsAsync(volunteer);

        _unitOfWorkMock
            .Setup(v => v.SaveChanges(_ct))
            .Returns(Task.CompletedTask);

        _validatorMock
            .Setup(v => v.ValidateAsync(command, _ct))
            .ReturnsAsync(validationResult);

        // act
        var result = await _handler.Handle(command, _ct);

        // assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.First(p => p.Id == pet.Id).Photos.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenVolunteerDoesNotExist_ShouldReturnError()
    {
        // arrange
        var validationResult = new ValidationResult();

        var volunteerId = VolunteerId.NewVolunteerId();

        var petId = PetId.NewPetId();

        var files = CreateUploadFileDtos(2);

        var command = new UploadFilesToPetCommand(volunteerId, petId, files);

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

    [Fact]
    public async Task Handle_ShouldReturnValidationError()
    {
        // arrange
        var volunteerId = VolunteerId.NewVolunteerId();

        var petId = PetId.NewPetId();

        var files = CreateUploadFileDtos(2);

        var command = new UploadFilesToPetCommand(volunteerId, petId, files);

        var errors = new List<ValidationFailure>()
        {
            new(nameof(command.Files), Errors.General.ValueIsInvalid(nameof(command.Files)).Serialize())
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
        error.InvalidField.Should().Be(nameof(command.Files));
    }

    [Fact]
    public async Task Handle_ShouldReturnFileUploadError()
    {
        // arrange
        var validationResult = new ValidationResult();

        var volunteer = CreateVolunteerWithPets(1);

        var pet = volunteer.Pets[0];

        var files = CreateUploadFileDtos(2);

        var command = new UploadFilesToPetCommand(volunteer.Id.Value, pet.Id.Value, files);

        _fileProviderMock
            .Setup(f => f.UploadFiles(It.IsAny<List<FileData>>(), _ct))
            .ReturnsAsync(Error.Failure("file.upload", "Fail to upload files in minio"));

        _volunteersRepositoryMock
            .Setup(v => v.GetById(volunteer.Id, _ct))
            .ReturnsAsync(volunteer);

        _unitOfWorkMock
            .Setup(v => v.SaveChanges(_ct))
            .Returns(Task.CompletedTask);

        _validatorMock
            .Setup(v => v.ValidateAsync(command, _ct))
            .ReturnsAsync(validationResult);

        // act
        var result = await _handler.Handle(command, _ct);

        // assert
        result.IsFailure.Should().BeTrue();

        var error = result.Error.First();
        error.Type.Should().Be(ErrorType.Failure);
        error.Code.Should().Be("file.upload");
        error.Message.Should().Be("Fail to upload files in minio");
    }

    private List<UploadFileDto> CreateUploadFileDtos(int count)
    {
        const string fileName = "test.jpg";

        var stream = new MemoryStream();

        var uploadFileDto = new UploadFileDto(stream, fileName);

        List<UploadFileDto> files = [];

        for (int i = 0; i < count; i++)
        {
            files.Add(uploadFileDto);
        }

        return files;
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
        var speciesId = SpeciesId.NewSpeciesId();
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