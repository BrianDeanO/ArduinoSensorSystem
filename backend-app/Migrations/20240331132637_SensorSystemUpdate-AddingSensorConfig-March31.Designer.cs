﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backEndApp.Data;

#nullable disable

namespace backEndApp.Migrations
{
    [DbContext(typeof(SensorSystemContext))]
    [Migration("20240331132637_SensorSystemUpdate-AddingSensorConfig-March31")]
    partial class SensorSystemUpdateAddingSensorConfigMarch31
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("backEndApp.Models.Device", b =>
                {
                    b.Property<int>("DeviceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeviceID"));

                    b.Property<string>("DeviceIdent")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("DeviceIsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DeviceLastSeen")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeviceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DeviceUpdateInterval")
                        .HasColumnType("int");

                    b.Property<string>("DeviceZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeviceID");

                    b.HasIndex("DeviceIdent")
                        .IsUnique();

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("backEndApp.Models.Sensor", b =>
                {
                    b.Property<int>("SensorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SensorID"));

                    b.Property<int>("ChannelCount")
                        .HasColumnType("int");

                    b.Property<int>("DeviceID")
                        .HasColumnType("int");

                    b.Property<string>("SensorIdent")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("SensorIsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("SensorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SensorType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SensorID");

                    b.HasIndex("DeviceID");

                    b.HasIndex("SensorIdent")
                        .IsUnique();

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("backEndApp.Models.SensorConfig", b =>
                {
                    b.Property<int>("SensorConfigID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SensorConfigID"));

                    b.Property<string>("SensorConfigKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SensorConfigValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SensorID")
                        .HasColumnType("int");

                    b.HasKey("SensorConfigID");

                    b.HasIndex("SensorID");

                    b.ToTable("SensorConfigs");
                });

            modelBuilder.Entity("backEndApp.Models.SensorData", b =>
                {
                    b.Property<int>("SensorDataID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SensorDataID"));

                    b.Property<int>("ChannelID")
                        .HasColumnType("int");

                    b.Property<string>("DataUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("DataValue")
                        .HasColumnType("real");

                    b.Property<int>("SensorID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeRecorded")
                        .HasColumnType("datetime2");

                    b.HasKey("SensorDataID");

                    b.HasIndex("SensorID");

                    b.ToTable("SensorDatas");
                });

            modelBuilder.Entity("backEndApp.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserFirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UserIsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("UserLastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UserNotifications")
                        .HasColumnType("bit");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("backEndApp.Models.UserDevice", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("DeviceID")
                        .HasColumnType("int");

                    b.HasKey("UserID", "DeviceID");

                    b.HasIndex("DeviceID");

                    b.ToTable("UserDevices");
                });

            modelBuilder.Entity("backEndApp.Models.Sensor", b =>
                {
                    b.HasOne("backEndApp.Models.Device", "Device")
                        .WithMany("Sensors")
                        .HasForeignKey("DeviceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });

            modelBuilder.Entity("backEndApp.Models.SensorConfig", b =>
                {
                    b.HasOne("backEndApp.Models.Sensor", "Sensor")
                        .WithMany("SensorConfigs")
                        .HasForeignKey("SensorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sensor");
                });

            modelBuilder.Entity("backEndApp.Models.SensorData", b =>
                {
                    b.HasOne("backEndApp.Models.Sensor", "Sensor")
                        .WithMany("SensorDatas")
                        .HasForeignKey("SensorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sensor");
                });

            modelBuilder.Entity("backEndApp.Models.UserDevice", b =>
                {
                    b.HasOne("backEndApp.Models.Device", "Device")
                        .WithMany("UserDevices")
                        .HasForeignKey("DeviceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backEndApp.Models.User", "User")
                        .WithMany("UserDevices")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backEndApp.Models.Device", b =>
                {
                    b.Navigation("Sensors");

                    b.Navigation("UserDevices");
                });

            modelBuilder.Entity("backEndApp.Models.Sensor", b =>
                {
                    b.Navigation("SensorConfigs");

                    b.Navigation("SensorDatas");
                });

            modelBuilder.Entity("backEndApp.Models.User", b =>
                {
                    b.Navigation("UserDevices");
                });
#pragma warning restore 612, 618
        }
    }
}