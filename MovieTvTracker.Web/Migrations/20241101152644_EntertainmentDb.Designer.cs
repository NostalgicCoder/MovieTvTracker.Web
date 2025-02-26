﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieTvTracker.Web.Data;

#nullable disable

namespace MovieTvTracker.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241101152644_EntertainmentDb")]
    partial class EntertainmentDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MovieTvTracker.Web.Data.FavoriteActor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("TMDBId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FavoriteActor");
                });

            modelBuilder.Entity("MovieTvTracker.Web.Data.WatchedMedia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FirstWatched")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastWatched")
                        .HasColumnType("datetime2");

                    b.Property<int>("TMDBId")
                        .HasColumnType("int");

                    b.Property<int>("WatchedQty")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WatchedMedia");
                });
#pragma warning restore 612, 618
        }
    }
}
