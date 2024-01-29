﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backEndApp.Models;

#nullable disable

namespace backEndApp.Migrations
{
    [DbContext(typeof(SensorSystemContext))]
    partial class SensorSystemContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DeviceID");

                    b.HasIndex("DeviceIdent")
                        .IsUnique();

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("backEndApp.Models.DeviceSensor", b =>
                {
                    b.Property<int>("DeviceID")
                        .HasColumnType("int");

                    b.Property<int>("SensorID")
                        .HasColumnType("int");

                    b.HasKey("DeviceID", "SensorID");

                    b.ToTable("DeviceSensors");
                });

            modelBuilder.Entity("backEndApp.Models.Sensor", b =>
                {
                    b.Property<int>("SensorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SensorID"));

                    b.Property<int>("ChannelCount")
                        .HasColumnType("int");

                    b.Property<string>("SensorIdent")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SensorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SensorType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SensorID");

                    b.HasIndex("SensorIdent")
                        .IsUnique();

                    b.ToTable("Sensors");
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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("DataValue")
                        .HasColumnType("real");

                    b.Property<int>("SensorID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeRecorded")
                        .HasColumnType("datetime2");

                    b.HasKey("SensorDataID");

                    b.ToTable("SensorDatas");
                });

            modelBuilder.Entity("backEndApp.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UserNotifications")
                        .HasColumnType("bit");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPhone")
                        .IsRequired()
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

                    b.ToTable("UserDevices");
                });
#pragma warning restore 612, 618
        }
    }
}
