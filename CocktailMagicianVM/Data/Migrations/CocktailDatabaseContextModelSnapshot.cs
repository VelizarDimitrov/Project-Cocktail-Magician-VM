﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(CocktailDatabaseContext))]
    partial class CocktailDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.Models.Bar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<byte[]>("BarCover");

                    b.Property<int?>("CityId");

                    b.Property<int?>("CountryId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Bars");
                });

            modelBuilder.Entity("Data.Models.BarCocktail", b =>
                {
                    b.Property<int>("BarId");

                    b.Property<int>("CocktailId");

                    b.HasKey("BarId", "CocktailId");

                    b.HasIndex("CocktailId");

                    b.ToTable("BarCocktail");
                });

            modelBuilder.Entity("Data.Models.BarComment", b =>
                {
                    b.Property<int>("BarId");

                    b.Property<int>("UserId");

                    b.Property<string>("Comment");

                    b.HasKey("BarId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("BarComment");
                });

            modelBuilder.Entity("Data.Models.BarRating", b =>
                {
                    b.Property<int>("BarId");

                    b.Property<int>("UserId");

                    b.Property<int>("Rating");

                    b.HasKey("BarId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("BarRating");
                });

            modelBuilder.Entity("Data.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Data.Models.Cocktail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<byte[]>("Photo");

                    b.HasKey("Id");

                    b.ToTable("Cocktails");
                });

            modelBuilder.Entity("Data.Models.CocktailComment", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("CocktailId");

                    b.Property<string>("Comment");

                    b.HasKey("UserId", "CocktailId");

                    b.HasIndex("CocktailId");

                    b.ToTable("CocktailComment");
                });

            modelBuilder.Entity("Data.Models.CocktailIngredient", b =>
                {
                    b.Property<int>("IngredientId");

                    b.Property<int>("CocktailId");

                    b.HasKey("IngredientId", "CocktailId");

                    b.HasIndex("CocktailId");

                    b.ToTable("CocktailIngredient");
                });

            modelBuilder.Entity("Data.Models.CocktailRating", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("CocktailId");

                    b.Property<int>("Rating");

                    b.HasKey("UserId", "CocktailId");

                    b.HasIndex("CocktailId");

                    b.ToTable("CocktailRating");
                });

            modelBuilder.Entity("Data.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Data.Models.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<byte>("Primary");

                    b.HasKey("Id");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("Data.Models.Notification", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<byte>("Seen");

                    b.Property<DateTime>("SentOn");

                    b.Property<string>("Text");

                    b.Property<int>("UserId");

                    b.HasKey("id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountStatus");

                    b.Property<string>("AccountType");

                    b.Property<int?>("CityId");

                    b.Property<int?>("CountryId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<byte[]>("UserPhoto");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Data.Models.UserBar", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("BarId");

                    b.HasKey("UserId", "BarId");

                    b.HasIndex("BarId");

                    b.ToTable("UserBar");
                });

            modelBuilder.Entity("Data.Models.UserCocktail", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("CocktailId");

                    b.HasKey("UserId", "CocktailId");

                    b.HasIndex("CocktailId");

                    b.ToTable("UserCocktail");
                });

            modelBuilder.Entity("Data.Models.Bar", b =>
                {
                    b.HasOne("Data.Models.City", "City")
                        .WithMany("Bars")
                        .HasForeignKey("CityId");

                    b.HasOne("Data.Models.Country", "Country")
                        .WithMany("Bars")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("Data.Models.BarCocktail", b =>
                {
                    b.HasOne("Data.Models.Bar", "Bar")
                        .WithMany("Cocktails")
                        .HasForeignKey("BarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.Cocktail", "Cocktail")
                        .WithMany("Bars")
                        .HasForeignKey("CocktailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.BarComment", b =>
                {
                    b.HasOne("Data.Models.Bar", "Bar")
                        .WithMany("Comments")
                        .HasForeignKey("BarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.User", "User")
                        .WithMany("BarComments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.BarRating", b =>
                {
                    b.HasOne("Data.Models.Bar", "Bar")
                        .WithMany("Ratings")
                        .HasForeignKey("BarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.User", "User")
                        .WithMany("BarRatings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.City", b =>
                {
                    b.HasOne("Data.Models.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("Data.Models.CocktailComment", b =>
                {
                    b.HasOne("Data.Models.Cocktail", "Cocktail")
                        .WithMany("Comments")
                        .HasForeignKey("CocktailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.User", "User")
                        .WithMany("CocktailComments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.CocktailIngredient", b =>
                {
                    b.HasOne("Data.Models.Cocktail", "Cocktail")
                        .WithMany("Ingredients")
                        .HasForeignKey("CocktailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.Ingredient", "Ingredient")
                        .WithMany("Cocktails")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.CocktailRating", b =>
                {
                    b.HasOne("Data.Models.Cocktail", "Cocktail")
                        .WithMany("Ratings")
                        .HasForeignKey("CocktailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.User", "User")
                        .WithMany("CocktailRatings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.Notification", b =>
                {
                    b.HasOne("Data.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.User", b =>
                {
                    b.HasOne("Data.Models.City", "City")
                        .WithMany("Users")
                        .HasForeignKey("CityId");

                    b.HasOne("Data.Models.Country", "Country")
                        .WithMany("Users")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("Data.Models.UserBar", b =>
                {
                    b.HasOne("Data.Models.Bar", "Bar")
                        .WithMany("FavoritedBy")
                        .HasForeignKey("BarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.User", "User")
                        .WithMany("FavoriteBars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Data.Models.UserCocktail", b =>
                {
                    b.HasOne("Data.Models.Cocktail", "Cocktail")
                        .WithMany("FavoritedBy")
                        .HasForeignKey("CocktailId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Data.Models.User", "User")
                        .WithMany("FavoriteCocktails")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
