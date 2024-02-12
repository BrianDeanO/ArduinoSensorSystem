using Microsoft.EntityFrameworkCore;
using backEndApp.Models;

namespace backEndApp.Data;
    public class SensorSystemContext : DbContext
    {
        public SensorSystemContext(DbContextOptions<SensorSystemContext> options) : 
            base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            
            // Creating composite key table
            modelBuilder.Entity<UserDevice>()
                .HasKey(ud => new {ud.UserID, ud.DeviceID});

            // Configures User + Device, many-to-many relationship
            modelBuilder.Entity<UserDevice>()
                    .HasOne(u => u.User)
                    .WithMany(ud => ud.UserDevices)
                    .HasForeignKey(u => u.UserID);
            modelBuilder.Entity<UserDevice>()
                    .HasOne(d => d.Device)
                    .WithMany(ud => ud.UserDevices)
                    .HasForeignKey(d => d.DeviceID);

            // Configures Device + Sensors, one-to-many relationship
            modelBuilder.Entity<Device>()
                .HasMany(d => d.Sensors)
                .WithOne(s => s.Device);

            // Configures Device + Sensors, one-to-many relationship
            modelBuilder.Entity<Sensor>()
                .HasOne(s => s.Device)
                .WithMany(d => d.Sensors)
                .HasForeignKey(d => d.DeviceID);
        }
    }

