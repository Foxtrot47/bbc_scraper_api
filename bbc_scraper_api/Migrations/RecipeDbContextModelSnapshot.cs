﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bbc_scraper_api.Contexts;

#nullable disable

namespace bbc_scraper_api.Migrations
{
    [DbContext(typeof(RecipeDbContext))]
    partial class RecipeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Alt")
                        .HasColumnType("longtext");

                    b.Property<double>("AspectRatio")
                        .HasColumnType("double");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("IngredientGroupId")
                        .HasColumnType("int");

                    b.Property<string>("IngredientText")
                        .HasColumnType("longtext");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.Property<string>("QuantityText")
                        .HasColumnType("longtext");

                    b.Property<string>("TermId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("IngredientGroupId");

                    b.HasIndex("TermId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.IngredientGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Heading")
                        .HasColumnType("longtext");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("IngredientGroups");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Instructions");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.NutritionalInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Label")
                        .HasColumnType("longtext");

                    b.Property<string>("Prefix")
                        .HasColumnType("longtext");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("Suffix")
                        .HasColumnType("longtext");

                    b.Property<double?>("Value")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("NutritionalInfos");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Avg")
                        .HasColumnType("double");

                    b.Property<bool>("IsHalfStar")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int?>("RatingId")
                        .HasColumnType("int");

                    b.Property<string>("SkillLevel")
                        .HasColumnType("longtext");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("TimeId")
                        .HasColumnType("int");

                    b.Property<string>("Yield")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("RatingId");

                    b.HasIndex("TimeId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeCategory", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("RecipeId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("RecipeCategories");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeCuisine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeCuisines");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeDiet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeDiets");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeKeyword", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("KeywordId")
                        .HasColumnType("int");

                    b.HasKey("RecipeId", "KeywordId");

                    b.HasIndex("KeywordId");

                    b.ToTable("RecipeKeywords");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.SimilarRecipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ImageId")
                        .HasColumnType("int");

                    b.Property<int?>("RatingId")
                        .HasColumnType("int");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("Url")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("RatingId");

                    b.HasIndex("RecipeId");

                    b.ToTable("SimilarRecipes");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.SimilarRecipeRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("HasRatingCount")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsHalfStar")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("RatingCount")
                        .HasColumnType("longtext");

                    b.Property<string>("RatingTypeLabel")
                        .HasColumnType("longtext");

                    b.Property<string>("RatingValue")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("SimilarRecipeRatings");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Term", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Display")
                        .HasColumnType("longtext");

                    b.Property<string>("Slug")
                        .HasColumnType("longtext");

                    b.Property<string>("Taxonomy")
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Term");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Time", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CookTime")
                        .HasColumnType("int");

                    b.Property<int>("PrepTime")
                        .HasColumnType("int");

                    b.Property<int>("TotalTime")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Time");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Ingredient", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.IngredientGroup", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("IngredientGroupId");

                    b.HasOne("bbc_scraper_api.MariaDBModels.Term", "Term")
                        .WithMany()
                        .HasForeignKey("TermId");

                    b.Navigation("Term");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.IngredientGroup", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Instruction", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany("Instructions")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.NutritionalInfo", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany("NutritionalInfo")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Recipe", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("bbc_scraper_api.MariaDBModels.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");

                    b.HasOne("bbc_scraper_api.MariaDBModels.Time", "Time")
                        .WithMany()
                        .HasForeignKey("TimeId");

                    b.Navigation("Image");

                    b.Navigation("Rating");

                    b.Navigation("Time");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeCategory", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Category", "Category")
                        .WithMany("RecipeCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", "Recipe")
                        .WithMany("Categories")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeCuisine", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany("Cuisine")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeDiet", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany("Diet")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.RecipeKeyword", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Keyword", "Keyword")
                        .WithMany("RecipeKeywords")
                        .HasForeignKey("KeywordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", "Recipe")
                        .WithMany("Keywords")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Keyword");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.SimilarRecipe", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("bbc_scraper_api.MariaDBModels.SimilarRecipeRating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany("SimilarRecipes")
                        .HasForeignKey("RecipeId");

                    b.Navigation("Image");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Category", b =>
                {
                    b.Navigation("RecipeCategories");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.IngredientGroup", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Keyword", b =>
                {
                    b.Navigation("RecipeKeywords");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Recipe", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Cuisine");

                    b.Navigation("Diet");

                    b.Navigation("Ingredients");

                    b.Navigation("Instructions");

                    b.Navigation("Keywords");

                    b.Navigation("NutritionalInfo");

                    b.Navigation("SimilarRecipes");
                });
#pragma warning restore 612, 618
        }
    }
}