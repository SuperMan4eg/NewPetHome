﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewPetHome.Volunteers.Infrastructure.DbContexts;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewPetHome.Volunteers.Infrastructure.Migrations
{
    [DbContext(typeof(VolunteersWriteDbContext))]
    partial class VolunteersWriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NewPetHome.Volunteers.Domain.Entities.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("birth_date");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<bool>("IsCastrated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_castrated");

                    b.Property<bool>("IsVaccinated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vaccinated");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<Guid?>("volunteer_id")
                        .HasColumnType("uuid")
                        .HasColumnName("volunteer_id");

                    b.ComplexProperty<Dictionary<string, object>>("Address", "NewPetHome.Volunteers.Domain.Entities.Pet.Address#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("city");

                            b1.Property<int>("HouseNumber")
                                .HasColumnType("integer")
                                .HasColumnName("house_number");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("street");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Color", "NewPetHome.Volunteers.Domain.Entities.Pet.Color#Color", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("color_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "NewPetHome.Volunteers.Domain.Entities.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("HealthInfo", "NewPetHome.Volunteers.Domain.Entities.Pet.HealthInfo#HealthInfo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("health_info_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Height", "NewPetHome.Volunteers.Domain.Entities.Pet.Height#Height", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<double>("Value")
                                .HasColumnType("double precision")
                                .HasColumnName("height_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "NewPetHome.Volunteers.Domain.Entities.Pet.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "NewPetHome.Volunteers.Domain.Entities.Pet.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)")
                                .HasColumnName("phone_number_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Position", "NewPetHome.Volunteers.Domain.Entities.Pet.Position#Position", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("position_value");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Weight", "NewPetHome.Volunteers.Domain.Entities.Pet.Weight#Weight", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<double>("Value")
                                .HasColumnType("double precision")
                                .HasColumnName("weight_value");
                        });

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.HasIndex("volunteer_id")
                        .HasDatabaseName("ix_pets_volunteer_id");

                    b.ToTable("pets", (string)null);
                });

            modelBuilder.Entity("NewPetHome.Volunteers.Domain.Entities.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Requisites")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("requisites");

                    b.Property<string>("SocialNetworks")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("social_networks");

                    b.Property<bool>("_isDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "NewPetHome.Volunteers.Domain.Entities.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "NewPetHome.Volunteers.Domain.Entities.Volunteer.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Experience", "NewPetHome.Volunteers.Domain.Entities.Volunteer.Experience#Experience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("experience");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "NewPetHome.Volunteers.Domain.Entities.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("last_name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "NewPetHome.Volunteers.Domain.Entities.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)")
                                .HasColumnName("phone_number");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("volunteers", (string)null);
                });

            modelBuilder.Entity("NewPetHome.Volunteers.Domain.Entities.Pet", b =>
                {
                    b.HasOne("NewPetHome.Volunteers.Domain.Entities.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("volunteer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_pets_volunteers_volunteer_id");

                    b.OwnsOne("NewPetHome.SharedKernel.ValueObjects.TypeDetails", "TypeDetails", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<Guid>("BreedId")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");

                            b1.Property<Guid>("SpecieId")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");
                        });

                    b.OwnsOne("NewPetHome.SharedKernel.ValueObjects.ValueObjectList<NewPetHome.SharedKernel.ValueObjects.Photo>", "Photos", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("photos");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("NewPetHome.SharedKernel.ValueObjects.Photo", "Values", b2 =>
                                {
                                    b2.Property<Guid>("ValueObjectListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<bool>("IsMain")
                                        .HasColumnType("boolean");

                                    b2.Property<string>("Path")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("ValueObjectListPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("photos");

                                    b2.WithOwner()
                                        .HasForeignKey("ValueObjectListPetId")
                                        .HasConstraintName("fk_pets_pets_value_object_list_pet_id");
                                });

                            b1.Navigation("Values");
                        });

                    b.OwnsOne("NewPetHome.SharedKernel.ValueObjects.ValueObjectList<NewPetHome.SharedKernel.ValueObjects.Requisite>", "Requisites", b1 =>
                        {
                            b1.Property<Guid>("PetId")
                                .HasColumnType("uuid");

                            b1.HasKey("PetId");

                            b1.ToTable("pets");

                            b1.ToJson("requisites");

                            b1.WithOwner()
                                .HasForeignKey("PetId")
                                .HasConstraintName("fk_pets_pets_id");

                            b1.OwnsMany("NewPetHome.SharedKernel.ValueObjects.Requisite", "Values", b2 =>
                                {
                                    b2.Property<Guid>("ValueObjectListPetId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("Id")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("Description")
                                        .IsRequired()
                                        .HasMaxLength(2000)
                                        .HasColumnType("character varying(2000)");

                                    b2.Property<string>("Name")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)");

                                    b2.HasKey("ValueObjectListPetId", "Id");

                                    b2.ToTable("pets");

                                    b2.ToJson("requisites");

                                    b2.WithOwner()
                                        .HasForeignKey("ValueObjectListPetId")
                                        .HasConstraintName("fk_pets_pets_value_object_list_pet_id");
                                });

                            b1.Navigation("Values");
                        });

                    b.Navigation("Photos")
                        .IsRequired();

                    b.Navigation("Requisites")
                        .IsRequired();

                    b.Navigation("TypeDetails")
                        .IsRequired();
                });

            modelBuilder.Entity("NewPetHome.Volunteers.Domain.Entities.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
