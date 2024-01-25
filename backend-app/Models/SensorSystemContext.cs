using Microsoft.EntityFrameworkCore;

namespace backEndApp.Models;

    public class SensorSystemContext : DbContext
    {
        public SensorSystemContext(DbContextOptions<SensorSystemContext> options) : 
            base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorDatas { get; set; }
        public DbSet<DeviceSensor> DeviceSensors { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //     optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=;Trusted_Connection=True;");
        // }
        // Put something here about the many to many tables -- stack overflow
    }

