﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewPetHome.Species.Infrastructure.DbContexts;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NewPetHome.Species.Infrastructure.Migrations
{
    [DbContext(typeof(SpeciesWriteDbContext))]
    partial class SpeciesWriteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NewPetHome.Species.Domain.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("SpecieId")
                        .HasColumnType("uuid")
                        .HasColumnName("specie_id");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "NewPetHome.Species.Domain.Breed.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_breeds");

                    b.HasIndex("SpecieId")
                        .HasDatabaseName("ix_breeds_specie_id");

                    b.ToTable("breeds", (string)null);
                });

            modelBuilder.Entity("NewPetHome.Species.Domain.Specie", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.ComplexProperty<Dictionary<string, object>>("Name", "NewPetHome.Species.Domain.Specie.Name#Name", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("species", (string)null);
                });

            modelBuilder.Entity("NewPetHome.Species.Domain.Breed", b =>
                {
                    b.HasOne("NewPetHome.Species.Domain.Specie", null)
                        .WithMany("Breeds")
                        .HasForeignKey("SpecieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_breeds_species_specie_id");
                });

            modelBuilder.Entity("NewPetHome.Species.Domain.Specie", b =>
                {
                    b.Navigation("Breeds");
                });
#pragma warning restore 612, 618
        }
    }
}