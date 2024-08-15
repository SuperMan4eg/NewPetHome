using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Domain;
using NewPetHome.Domain.Shared;
using NewPetHome.Domain.Volunteers;

namespace NewPetHome.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id =>id.Value,
                value => VolunteerId.Create(value));
            
        builder.Property(v => v.FullName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        
        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);

        builder.Property(v => v.Experience)
            .IsRequired();
        
        builder.Property(v => v.CountPetsFindHome)
            .IsRequired();
    
        builder.Property(v => v.CountPetsLookingHome)
            .IsRequired();
        
        builder.Property(v => v.CountPetsInTreatment)
            .IsRequired();
        
        builder.Property(v => v.PhoneNumber)
            .IsRequired()
            .HasMaxLength(Constants.MAX_PHONE_NUMBER_LENGTH);
        
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id");

        builder.OwnsOne(v => v.Details, db =>
        {
            db.ToJson();

            db.OwnsMany(d => d.Requisites, rb =>
            {
                rb.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                rb.Property(d => d.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
            });

            db.OwnsMany(d => d.SocialNetworks, sb =>
            {
                sb.Property(d => d.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                sb.Property(d => d.Url)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
            });
        });
    }
}