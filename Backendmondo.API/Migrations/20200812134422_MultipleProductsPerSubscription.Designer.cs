﻿// <auto-generated />
using System;
using Backendmondo.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Backendmondo.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200812134422_MultipleProductsPerSubscription")]
    partial class MultipleProductsPerSubscription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Backendmondo.API.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("DurationMonths")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<float>("PriceUSD")
                        .HasColumnType("real");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("uuid");

                    b.Property<float>("TaxUSD")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Backendmondo.API.Models.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Purchased")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Backendmondo.API.Models.SubscriptionPause", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Ended")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Started")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("SubscriptionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("SubscriptionPauses");
                });

            modelBuilder.Entity("Backendmondo.API.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backendmondo.API.Models.Product", b =>
                {
                    b.HasOne("Backendmondo.API.Models.Subscription", null)
                        .WithMany("Products")
                        .HasForeignKey("SubscriptionId");
                });

            modelBuilder.Entity("Backendmondo.API.Models.Subscription", b =>
                {
                    b.HasOne("Backendmondo.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Backendmondo.API.Models.SubscriptionPause", b =>
                {
                    b.HasOne("Backendmondo.API.Models.Subscription", "Subscription")
                        .WithMany("Pauses")
                        .HasForeignKey("SubscriptionId");
                });
#pragma warning restore 612, 618
        }
    }
}