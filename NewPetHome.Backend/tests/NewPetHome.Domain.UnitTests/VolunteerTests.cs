using FluentAssertions;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.ValueObjects;

namespace NewPetHome.Domain.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void AddPet_WithEmptyPets_ReturnSuccessResult()
    {
        // arrange
        var volunteer = CreateVolunteerWithPets(0);

        var petId = PetId.NewPetId();
        var name = Name.Create("test").Value;
        var description = Description.Create("test").Value;
        var speciesId = SpeciesId.NewSpeciesId();
        var breedId = BreedId.NewBreedId();
        var typeDetails = TypeDetails.Create(speciesId, breedId).Value;
        var color = Color.Create("test").Value;
        var health = HealthInfo.Create("test").Value;
        var address = Address.Create("test", "test", 1).Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var phoneNumber = PhoneNumber.Create("9999999999").Value;
        var isCastrated = true;
        var birthDate = DateTime.Now;
        var isVaccinated = true;
        var status = PetStatus.FindHome;
        var photos = new ValueObjectList<Photo>(new List<Photo>());
        var requisites = new ValueObjectList<Requisite>(new List<Requisite>());

        var pet = new Pet(
            petId,
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

        // act
        var result = volunteer.AddPet(pet);

        // assert
        var addedPetResult = volunteer.GetPetById(petId);


        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(pet.Id);
        addedPetResult.Value.Position.Should().Be(Position.First);
    }

    [Fact]
    public void AddPet_WithOtherPets_ReturnSuccessResult()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var petId = PetId.NewPetId();
        var name = Name.Create("test").Value;
        var description = Description.Create("test").Value;
        var speciesId = SpeciesId.NewSpeciesId();
        var breedId = BreedId.NewBreedId();
        var typeDetails = TypeDetails.Create(speciesId, breedId).Value;
        var color = Color.Create("test").Value;
        var health = HealthInfo.Create("test").Value;
        var address = Address.Create("test", "test", 1).Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var phoneNumber = PhoneNumber.Create("9999999999").Value;
        var isCastrated = true;
        var birthDate = DateTime.Now;
        var isVaccinated = true;
        var status = PetStatus.FindHome;
        var photos = new ValueObjectList<Photo>(new List<Photo>());
        var requisites = new ValueObjectList<Requisite>(new List<Requisite>());

        var petToAdd = new Pet(
            petId,
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

        // act
        var result = volunteer.AddPet(petToAdd);

        // assert
        var addedPetResult = volunteer.GetPetById(petId);

        var position = Position.Create(petsCount + 1).Value;

        result.IsSuccess.Should().BeTrue();
        addedPetResult.IsSuccess.Should().BeTrue();
        addedPetResult.Value.Id.Should().Be(petToAdd.Id);
        addedPetResult.Value.Position.Should().Be(position);
    }

    [Fact]
    public void MovePet_WhenPetAlreadyAtNewPosition_ShouldNotMove()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(secondPet, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(2);
        thirdPet.Position.Value.Should().Be(3);
        fourthPet.Position.Value.Should().Be(4);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MovePet_WhenNewPositionIsLower_ShouldMoveOtherPetsForward()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(fourthPet, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MovePet_WhenNewPositionIsGreater_ShouldMoveOtherPetsBack()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fourthPosition = Position.Create(4).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(secondPet, fourthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }
    
    [Fact]
    public void MovePet_WhenNewPositionIsFirst_ShouldMoveOtherPetsForward()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var firstPosition = Position.Create(1).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(fifthPet, firstPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }
    
    [Fact]
    public void MovePet_WhenNewPositionIsLast_ShouldMoveOtherPetsBack()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fifthPosition = Position.Create(5).Value;

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        // act
        var result = volunteer.MovePet(firstPet, fifthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(5);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(4);
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