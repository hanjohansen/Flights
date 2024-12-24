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
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Flights.Domain.Entities.Game.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("FinishAfterFirstRank")
                        .HasColumnType("INTEGER");

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

                    b.Property<Guid?>("TournamentGameId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("X01Target")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TournamentGameId")
                        .IsUnique();

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.Game.GamePlayerEntity", b =>
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

            modelBuilder.Entity("Flights.Domain.Entities.Game.GameRoundEntity", b =>
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

            modelBuilder.Entity("Flights.Domain.Entities.Game.RoundStatEntity", b =>
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

            modelBuilder.Entity("Flights.Domain.Entities.PlayerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("players", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.PlayerFileEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("StoragePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("player_files", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentEntity", b =>
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

                    b.Property<bool>("SemiFinalWithLosersCup")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Started")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("X01Target")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("tournaments", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentGameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsLosersCup")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("TournamentRoundId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TournamentRoundId");

                    b.ToTable("tournament_games", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentPlayerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Rank")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TournamentId");

                    b.ToTable("tournament_players", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentRoundEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("WildCardId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.HasIndex("WildCardId");

                    b.ToTable("tournament_rounds", (string)null);
                });

            modelBuilder.Entity("Flights.Domain.Entities.Game.GameEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.Tournament.TournamentGameEntity", "TournamentGame")
                        .WithOne("Game")
                        .HasForeignKey("Flights.Domain.Entities.Game.GameEntity", "TournamentGameId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("TournamentGame");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Game.GamePlayerEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.Game.GameEntity", "Game")
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

            modelBuilder.Entity("Flights.Domain.Entities.Game.GameRoundEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.Game.GameEntity", "Game")
                        .WithMany("Rounds")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Game.RoundStatEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.PlayerEntity", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flights.Domain.Entities.Game.GameRoundEntity", "Round")
                        .WithMany("RoundStats")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Flights.Domain.Entities.Game.DartStatEntity", "FirstDart", b1 =>
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

                    b.OwnsOne("Flights.Domain.Entities.Game.DartStatEntity", "SecondDart", b1 =>
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

                    b.OwnsOne("Flights.Domain.Entities.Game.DartStatEntity", "ThirdDart", b1 =>
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

            modelBuilder.Entity("Flights.Domain.Entities.PlayerFileEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.PlayerEntity", "Player")
                        .WithMany("Files")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentGameEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.Tournament.TournamentRoundEntity", "TournamentRound")
                        .WithMany("Games")
                        .HasForeignKey("TournamentRoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TournamentRound");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentPlayerEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.PlayerEntity", "Player")
                        .WithMany("Tournaments")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flights.Domain.Entities.Tournament.TournamentEntity", "Tournament")
                        .WithMany("Players")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentRoundEntity", b =>
                {
                    b.HasOne("Flights.Domain.Entities.Tournament.TournamentEntity", "Tournament")
                        .WithMany("Rounds")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flights.Domain.Entities.Tournament.TournamentPlayerEntity", "WildCard")
                        .WithMany()
                        .HasForeignKey("WildCardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Tournament");

                    b.Navigation("WildCard");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Game.GameEntity", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Game.GameRoundEntity", b =>
                {
                    b.Navigation("RoundStats");
                });

            modelBuilder.Entity("Flights.Domain.Entities.PlayerEntity", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Games");

                    b.Navigation("Tournaments");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentEntity", b =>
                {
                    b.Navigation("Players");

                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentGameEntity", b =>
                {
                    b.Navigation("Game");
                });

            modelBuilder.Entity("Flights.Domain.Entities.Tournament.TournamentRoundEntity", b =>
                {
                    b.Navigation("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
