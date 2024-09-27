using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Domain.SpeciesManagement;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Infrastructure.Configurations.Write;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);

        builder.OwnsMany(s => s.Breeds, bb =>
        {
            bb.Property(b => b.Id)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value));

            bb.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
        });
    }
}