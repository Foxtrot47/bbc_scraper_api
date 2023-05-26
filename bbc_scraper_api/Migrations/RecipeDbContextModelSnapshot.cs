﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
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
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CategoryRecipe", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("integer");

                    b.Property<int>("RecipesId")
                        .HasColumnType("integer");

                    b.HasKey("CategoriesId", "RecipesId");

                    b.HasIndex("RecipesId");

                    b.ToTable("RecipeCategories", (string)null);
                });

            modelBuilder.Entity("CuisineRecipe", b =>
                {
                    b.Property<int>("CuisinesId")
                        .HasColumnType("integer");

                    b.Property<int>("RecipesId")
                        .HasColumnType("integer");

                    b.HasKey("CuisinesId", "RecipesId");

                    b.HasIndex("RecipesId");

                    b.ToTable("RecipeCuisines", (string)null);
                });

            modelBuilder.Entity("DietRecipe", b =>
                {
                    b.Property<int>("DietsId")
                        .HasColumnType("integer");

                    b.Property<int>("RecipesId")
                        .HasColumnType("integer");

                    b.HasKey("DietsId", "RecipesId");

                    b.HasIndex("RecipesId");

                    b.ToTable("RecipeDiets", (string)null);
                });

            modelBuilder.Entity("KeywordRecipe", b =>
                {
                    b.Property<int>("KeywordsId")
                        .HasColumnType("integer");

                    b.Property<int>("RecipesId")
                        .HasColumnType("integer");

                    b.HasKey("KeywordsId", "RecipesId");

                    b.HasIndex("RecipesId");

                    b.ToTable("RecipeKeywords", (string)null);
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Cuisine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cuisines");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Diet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Diets");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Alt")
                        .HasColumnType("text");

                    b.Property<double>("AspectRatio")
                        .HasColumnType("double precision");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("IngredientGroupId")
                        .HasColumnType("integer");

                    b.Property<string>("IngredientText")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("QuantityText")
                        .HasColumnType("text");

                    b.Property<int?>("TermId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IngredientGroupId");

                    b.HasIndex("TermId");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.IngredientGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Heading")
                        .HasColumnType("text");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("IngredientGroups");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("RecipeId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Instructions");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Keyword", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.NutritionalInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Label")
                        .HasColumnType("text");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("integer");

                    b.Property<string>("Suffix")
                        .HasColumnType("text");

                    b.Property<double?>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("NutritionalInfos");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Avg")
                        .HasColumnType("double precision");

                    b.Property<bool>("IsHalfStar")
                        .HasColumnType("boolean");

                    b.Property<int>("Total")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("ImageId")
                        .HasColumnType("integer");

                    b.Property<int?>("IngredientId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("RatingId")
                        .HasColumnType("integer");

                    b.Property<string>("SkillLevel")
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<int?>("TimeId")
                        .HasColumnType("integer");

                    b.Property<string>("Yield")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RatingId");

                    b.HasIndex("TimeId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.SimilarRecipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ImageId")
                        .HasColumnType("integer");

                    b.Property<int?>("RatingId")
                        .HasColumnType("integer");

                    b.Property<int?>("RecipeId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("RatingId");

                    b.HasIndex("RecipeId");

                    b.ToTable("SimilarRecipes");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Term", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Display")
                        .HasColumnType("text");

                    b.Property<string>("Slug")
                        .HasColumnType("text");

                    b.Property<string>("Taxonomy")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Terms");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Time", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CookTime")
                        .HasColumnType("integer");

                    b.Property<int>("PrepTime")
                        .HasColumnType("integer");

                    b.Property<int>("TotalTime")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Times");
                });

            modelBuilder.Entity("CategoryRecipe", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany()
                        .HasForeignKey("RecipesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CuisineRecipe", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Cuisine", null)
                        .WithMany()
                        .HasForeignKey("CuisinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany()
                        .HasForeignKey("RecipesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DietRecipe", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Diet", null)
                        .WithMany()
                        .HasForeignKey("DietsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany()
                        .HasForeignKey("RecipesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("KeywordRecipe", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Keyword", null)
                        .WithMany()
                        .HasForeignKey("KeywordsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany()
                        .HasForeignKey("RecipesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

                    b.HasOne("bbc_scraper_api.MariaDBModels.Ingredient", null)
                        .WithMany("Recipes")
                        .HasForeignKey("IngredientId");

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

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.SimilarRecipe", b =>
                {
                    b.HasOne("bbc_scraper_api.MariaDBModels.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.HasOne("bbc_scraper_api.MariaDBModels.Rating", "Rating")
                        .WithMany()
                        .HasForeignKey("RatingId");

                    b.HasOne("bbc_scraper_api.MariaDBModels.Recipe", null)
                        .WithMany("SimilarRecipes")
                        .HasForeignKey("RecipeId");

                    b.Navigation("Image");

                    b.Navigation("Rating");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Ingredient", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.IngredientGroup", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("bbc_scraper_api.MariaDBModels.Recipe", b =>
                {
                    b.Navigation("Ingredients");

                    b.Navigation("Instructions");

                    b.Navigation("NutritionalInfo");

                    b.Navigation("SimilarRecipes");
                });
#pragma warning restore 612, 618
        }
    }
}
