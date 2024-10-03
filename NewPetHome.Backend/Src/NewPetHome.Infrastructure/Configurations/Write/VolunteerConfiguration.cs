using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewPetHome.Applications.Dtos;
using NewPetHome.Domain.Shared.ValueObjects;
using NewPetHome.Domain.VolunteersManagement.Entities;
using NewPetHome.Domain.VolunteersManagement.IDs;

namespace NewPetHome.Infrastructure.Configurations.Write;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName, fb =>
        {
            fb.Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("first_name");

            fb.Property(f => f.LastName)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("last_name");
        });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.ComplexProperty(v => v.Email, eb =>
        {
            eb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("email");
        });

        builder.ComplexProperty(v => v.Experience, eb =>
        {
            eb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("experience");
        });

        builder.ComplexProperty(v => v.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Domain.Shared.Constants.MAX_PHONE_NUMBER_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(v => v.Requisites)
            .HasConversion(
                requisites => JsonSerializer.Serialize(requisites
                    .Select(r => new RequisiteDto(r.Name, r.Description)), JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<RequisiteDto>>(json, JsonSerializerOptions.Default)!
                    .Select(r => Requisite.Create(r.Name, r.Description).Value)
                    .ToList(),
                new ValueComparer<IReadOnlyList<Requisite>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) =>
                        HashCode.Combine(a, v.GetHashCode())),
                    c => (IReadOnlyList<Requisite>)c.ToList()))
            .HasColumnType("jsonb")
            .HasColumnName("requisites");

        builder.Property(v => v.SocialNetworks)
            .HasConversion(
                socialNetworks => JsonSerializer.Serialize(socialNetworks
                    .Select(s => new SocialNetworkDto(s.Name, s.Url)), JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<SocialNetworkDto>>(json, JsonSerializerOptions.Default)!
                    .Select(s => SocialNetwork.Create(s.Name, s.Url).Value)
                    .ToList(),
                new ValueComparer<IReadOnlyList<SocialNetwork>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) =>
                        HashCode.Combine(a, v.GetHashCode())),
                    c => (IReadOnlyList<SocialNetwork>)c.ToList()))
            .HasColumnType("jsonb")
            .HasColumnName("social_networks");

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

        // builder.HasQueryFilter(v=>v.isDeleted == false);

        builder.Navigation(v => v.Pets).AutoInclude();
    }
}