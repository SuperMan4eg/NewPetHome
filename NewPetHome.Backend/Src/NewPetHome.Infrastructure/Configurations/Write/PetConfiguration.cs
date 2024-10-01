using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.SpeciesManagement.IDs;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.Enums;
using NewPetHome.Domain.VolunteersManagement.IDs;

namespace NewPetHome.Infrastructure.Configurations.Write;

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
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.Position, sb =>
        {
            sb.Property(n => n.Value)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_HIGH_TEXT_LENGTH);
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
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.HealthInfo, hb =>
        {
            hb.Property(h => h.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("city");

            ab.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("street");

            ab.Property(a => a.HouseNumber)
                .IsRequired()
                .HasColumnName("house_number");
        });

        builder.ComplexProperty(p => p.Weight, wb =>
        {
            wb.Property(w => w.Value)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.Height, hb =>
        {
            hb.Property(h => h.Value)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_PHONE_NUMBER_LENGTH);
        });

        builder.Property(p => p.IsCastrated)
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .IsRequired();

        builder.Property(p => p.IsVaccinated)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion(
                status => status.ToString(),
                value => (PetStatus)Enum.Parse(typeof(PetStatus), value!));

        builder.Property(p => p.CreatedDate)
            .IsRequired();

        builder.OwnsOne(p => p.Photos, pb =>
        {
            pb.ToJson();

            pb.OwnsMany(p => p.Values, phb =>
            {
                phb.Property(d => d.Path)
                    .HasConversion(
                        p => p.Path,
                        value => FilePath.Create(value).Value)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);

                phb.Property(p => p.IsMain)
                    .IsRequired();
            });
        });

        builder.OwnsOne(p => p.Requisites, rb =>
        {
            rb.ToJson();

            rb.OwnsMany(r => r.Values, reb =>
            {
                reb.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);

                reb.Property(d => d.Description)
                    .IsRequired()
                    .HasMaxLength(Domain.Shared.Constants.MAX_HIGH_TEXT_LENGTH);
            });
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}