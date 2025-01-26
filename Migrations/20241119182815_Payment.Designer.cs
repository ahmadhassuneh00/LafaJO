﻿// <auto-generated />
using System;
using FinalProjAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinalProjAPI.Migrations
{
    [DbContext(typeof(DataContextEF))]
    [Migration("20241119182815_Payment")]
    partial class Payment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FinalProjAPI.Models.BookTrip", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<decimal?>("TotalCost")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("numberOfPersons")
                        .HasColumnType("int");

                    b.HasKey("BookId");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("bookTrip", "FinalProjPost");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Car", b =>
                {
                    b.Property<int>("CarID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CarID"));

                    b.Property<string>("Color")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("DailyRate")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("FuelType")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("ImageURL")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("RegistrationId")
                        .HasColumnType("int");

                    b.Property<string>("TransmissionType")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("CarID");

                    b.HasIndex("RegistrationId");

                    b.ToTable("Cars", "FinalProjPost");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Company", b =>
                {
                    b.Property<int>("RegistrationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistrationID"));

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPersonName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeID")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("RegistrationID");

                    b.HasIndex("TypeID");

                    b.ToTable("Registration", "FinalProjCompanies");
                });

            modelBuilder.Entity("FinalProjAPI.Models.CompanyType", b =>
                {
                    b.Property<int>("TypeID")
                        .HasColumnType("int");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TypeID");

                    b.ToTable("CompanyTypes", "FinalProjCompanies");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemId"));

                    b.Property<byte[]>("ImageURL")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RegistrationId")
                        .HasColumnType("int");

                    b.Property<int?>("userId")
                        .HasColumnType("int");

                    b.HasKey("ItemId");

                    b.HasIndex("RegistrationId");

                    b.HasIndex("userId");

                    b.ToTable("Items", "FinalProjPost");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Payments", b =>
                {
                    b.Property<int>("paymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("paymentID"));

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cardHolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("cvv")
                        .HasColumnType("int");

                    b.Property<string>("expirationDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("paymentID");

                    b.ToTable("Payment", "FinalProjPost");
                });

            modelBuilder.Entity("FinalProjAPI.Models.RentCar", b =>
                {
                    b.Property<int>("RentalID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RentalID"));

                    b.Property<int>("CarID")
                        .HasColumnType("int");

                    b.Property<DateTime>("RentalEndDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("RentalStartDate")
                        .HasColumnType("date");

                    b.Property<decimal?>("TotalCost")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("RentalID");

                    b.HasIndex("CarID");

                    b.HasIndex("UserId");

                    b.ToTable("Rentals", "FinalProjPost");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ReviewID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<int>("CompanyRegistrationID")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("RegistrationId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReviewId");

                    b.HasIndex("CompanyRegistrationID");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews", "FinalProjPost");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Trip", b =>
                {
                    b.Property<int>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripId"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("ImageURL")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("NumOfTourist")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("RegistrationId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("TripId");

                    b.HasIndex("RegistrationId");

                    b.ToTable("Trips", "FinalProjPost");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users", "FinalProjUser");
                });

            modelBuilder.Entity("FinalProjAPI.Models.BookTrip", b =>
                {
                    b.HasOne("FinalProjAPI.Models.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinalProjAPI.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Car", b =>
                {
                    b.HasOne("FinalProjAPI.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("RegistrationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Company", b =>
                {
                    b.HasOne("FinalProjAPI.Models.CompanyType", "CompanyType")
                        .WithMany("Companies")
                        .HasForeignKey("TypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompanyType");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Item", b =>
                {
                    b.HasOne("FinalProjAPI.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("RegistrationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinalProjAPI.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("userId");

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinalProjAPI.Models.RentCar", b =>
                {
                    b.HasOne("FinalProjAPI.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinalProjAPI.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Review", b =>
                {
                    b.HasOne("FinalProjAPI.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyRegistrationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinalProjAPI.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinalProjAPI.Models.Trip", b =>
                {
                    b.HasOne("FinalProjAPI.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("RegistrationId");

                    b.Navigation("Company");
                });

            modelBuilder.Entity("FinalProjAPI.Models.CompanyType", b =>
                {
                    b.Navigation("Companies");
                });
#pragma warning restore 612, 618
        }
    }
}
