using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Domain.SpeciesManagement;
using NewPetHome.Domain.SpeciesManagement.IDs;

namespace NewPetHome.Infrastructure.Configurations.Write;

public class SpecieConfiguration : IEntityTypeConfiguration<Specie>
{
    public void Configure(EntityTypeBuilder<Specie> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpecieId.Create(value));

        builder.ComplexProperty(s => s.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey("specie_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.Breeds).AutoInclude();
    }
}