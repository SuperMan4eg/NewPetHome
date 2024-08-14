using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Domain;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.Species)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(p => p.Breed)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.Color)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.HealthInfo)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(p => p.Weight)
            .IsRequired();

        builder.Property(p => p.Height)
            .IsRequired();
        
        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_PHONE_NUMBER_LENGTH);

        builder.Property(p => p.IsCastrated)
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .IsRequired();
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired();
     
        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.HasMany(p => p.Requisites)
            .WithOne();

        builder.Property(p => p.CreatedDate)
            .IsRequired();

        builder.HasMany(p => p.Photos)
            .WithOne()
            .HasForeignKey("pet_id");
    }
}