﻿// <auto-generated />
using System;
using Flights.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Flights.Infrastructure.Data.Migrations
{
    [DbContext(typeof(FlightsDbContext))]
    partial class FlightsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Flights.Domain.Entities.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("Finished")
                        .HasColumnType("TEXT");

                    b.Property<string>("InModifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OutModifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Started")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("X01Target")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.GamePlayerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("game_players", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.GameRoundEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("game_rounds", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.PlayerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("players", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.RoundStatEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("EndPoints")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsBust")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsIn")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Rank")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("RoundId")
                        .HasColumnType("TEXT");

                    b.Property<int>("StartPoints")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("RoundId");

                    b.ToTable("round_stat", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.GamePlayerEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.GameEntity", "Game")
                        .WithMany("Players")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flights.Domain.Entities.PlayerEntity", "Player")
                        .WithMany("Games")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Flights.Domain.Entities.GameRoundEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.GameEntity", "Game")
                        .WithMany("Rounds")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Flights.Domain.Entities.RoundStatEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.PlayerEntity", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flights.Domain.Entities.GameRoundEntity", "Round")
                        .WithMany("RoundStats")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Flights.Domain.Entities.DartStatEntity", "FirstDart", b1 =>
                        {
                            b1.Property<Guid>("RoundStatEntityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Modifier")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<int>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("RoundStatEntityId");

                            b1.ToTable("round_stat");

                            b1.WithOwner()
                                .HasForeignKey("RoundStatEntityId");
                        });

                    b.OwnsOne("Flights.Domain.Entities.DartStatEntity", "SecondDart", b1 =>
                        {
                            b1.Property<Guid>("RoundStatEntityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Modifier")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<int>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("RoundStatEntityId");

                            b1.ToTable("round_stat");

                            b1.WithOwner()
                                .HasForeignKey("RoundStatEntityId");
                        });

                    b.OwnsOne("Flights.Domain.Entities.DartStatEntity", "ThirdDart", b1 =>
                        {
                            b1.Property<Guid>("RoundStatEntityId")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Modifier")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.Property<int>("Value")
                                .HasColumnType("INTEGER");

                            b1.HasKey("RoundStatEntityId");

                            b1.ToTable("round_stat");

                            b1.WithOwner()
                                .HasForeignKey("RoundStatEntityId");
                        });

                    b.Navigation("FirstDart");

                    b.Navigation("Player");

                    b.Navigation("Round");

                    b.Navigation("SecondDart");

                    b.Navigation("ThirdDart");
                });

            modelBuilder.Entity("Flights.Domain.Entities.GameEntity", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("Flights.Domain.Entities.GameRoundEntity", b =>
                {
                    b.Navigation("RoundStats");
                });

            modelBuilder.Entity("Flights.Domain.Entities.PlayerEntity", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
