using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Domain.SpeciesManagement;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value));
        
        builder.ComplexProperty(b => b.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
        });
    }
}