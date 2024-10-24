using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Core.Dtos;
using NewPetHome.Core.Extensions;
using NewPetHome.SharedKernel;
using NewPetHome.SharedKernel.ValueObjects;
using NewPetHome.SharedKernel.ValueObjects.Ids;
using NewPetHome.Volunteers.Domain.Entities;
using NewPetHome.Volunteers.Domain.Enums;

namespace NewPetHome.Volunteers.Infrastructure.Configurations.Write;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));

        builder.ComplexProperty(p => p.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });

        builder.ComplexProperty(p => p.Position, sb =>
        {
            sb.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("position");
        });

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.OwnsOne(p => p.TypeDetails, td =>
        {
            td.Property(t => t.SpecieId)
                .HasConversion(
                    id => id.Value,
                    value => SpecieId.Create(value))
                .HasColumnName("species_id");

            td.Property(t => t.BreedId)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value))
                .HasColumnName("breed_id");
        });

        builder.ComplexProperty(p => p.Color, cb =>
        {
            cb.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("color");
        });

        builder.ComplexProperty(p => p.HealthInfo, hb =>
        {
            hb.Property(h => h.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("health_info");
        });

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("city");

            ab.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("street");

            ab.Property(a => a.HouseNumber)
                .IsRequired()
                .HasColumnName("house_number");
        });

        builder.ComplexProperty(p => p.Weight, wb =>
        {
            wb.Property(w => w.Value)
                .IsRequired()
                .HasColumnName("weight");
        });

        builder.ComplexProperty(p => p.Height, hb =>
        {
            hb.Property(h => h.Value)
                .IsRequired()
                .HasColumnName("height");
        });

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_PHONE_NUMBER_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.Property(p => p.IsCastrated)
            .IsRequired()
            .HasColumnName("is_castrated");

        builder.Property(p => p.BirthDate)
            .IsRequired()
            .HasColumnName("birth_date");

        builder.Property(p => p.IsVaccinated)
            .IsRequired()
            .HasColumnName("is_vaccinated");

        builder.Property(p => p.Status)
            .HasConversion(
                status => status.ToString(),
                value => (PetStatus)Enum.Parse(typeof(PetStatus), value!))
            .HasColumnName("status");

        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasColumnName("created_date");

        builder.Property(v => v.Photos)
            .ValueObjectsCollectionJsonConversion(
                photo => new PetPhotoDto { PathToStorage = photo.Path.Path, IsMain = photo.IsMain },
                dto => Photo.Create(FilePath.Create(dto.PathToStorage).Value, dto.IsMain).Value)
            .HasColumnName("photos");

        builder.Property(v => v.Requisites)
            .ValueObjectsCollectionJsonConversion(
                requisite => new RequisiteDto(requisite.Description, requisite.Name),
                dto => Requisite.Create(dto.Name, dto.Description).Value)
            .HasColumnName("requisites");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}