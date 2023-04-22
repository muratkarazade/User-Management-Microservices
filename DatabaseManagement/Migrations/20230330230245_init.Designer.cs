﻿// <auto-generated />
using System;
using DatabaseManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatabaseManagement.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230330230245_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DatabaseManagement.Models.AuthenticationCredential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId1")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("UserId1")
                        .IsUnique()
                        .HasFilter("[UserId1] IS NOT NULL");

                    b.ToTable("AuthenticationCredentials");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "password1",
                            UserId = 1,
                            Username = "user1"
                        },
                        new
                        {
                            Id = 2,
                            Password = "password2",
                            UserId = 2,
                            Username = "user2"
                        },
                        new
                        {
                            Id = 3,
                            Password = "password3",
                            UserId = 3,
                            Username = "user3"
                        });
                });

            modelBuilder.Entity("DatabaseManagement.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "user1@example.com",
                            Name = "User",
                            Surname = "One"
                        },
                        new
                        {
                            Id = 2,
                            Email = "user2@example.com",
                            Name = "User",
                            Surname = "Two"
                        },
                        new
                        {
                            Id = 3,
                            Email = "user3@example.com",
                            Name = "User",
                            Surname = "Three"
                        });
                });

            modelBuilder.Entity("DatabaseManagement.Models.AuthenticationCredential", b =>
                {
                    b.HasOne("DatabaseManagement.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("DatabaseManagement.Models.AuthenticationCredential", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DatabaseManagement.Models.User", null)
                        .WithOne("AuthenticationCredential")
                        .HasForeignKey("DatabaseManagement.Models.AuthenticationCredential", "UserId1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DatabaseManagement.Models.User", b =>
                {
                    b.Navigation("AuthenticationCredential")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}