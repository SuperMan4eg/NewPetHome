using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Core.Dtos;

namespace NewPetHome.Species.Infrastructure.Configurations.Read;

public class SpecieDtoConfiguration: IEntityTypeConfiguration<SpecieDto>
{
        public void Configure(EntityTypeBuilder<SpecieDto> builder)
        {
            builder.ToTable("species");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired();
            
            builder.HasMany(s => s.Breeds)
                .WithOne()
                .HasForeignKey(b => b.SpecieId);
        }
}