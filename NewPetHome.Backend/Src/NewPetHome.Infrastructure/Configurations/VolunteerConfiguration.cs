using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Domain;
using NewPetHome.Domain.Shared;

namespace NewPetHome.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(m => m.Experience)
            .IsRequired();
        
        builder.Property(m => m.CountPetsFindHome)
            .IsRequired();
    
        builder.Property(m => m.CountPetsLookingHome)
            .IsRequired();
        
        builder.Property(m => m.CountPetsInTreatment)
            .IsRequired();
        
        builder.Property(m => m.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_PHONE_NUMBER_LENGTH);
        
        builder.HasMany(m => m.SocialNetworks)
            .WithOne()
            .HasForeignKey("volunteer_id");
        
        builder.HasMany(m => m.Requisites)
            .WithOne();
        
        builder.HasMany(m => m.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id");
    }
}