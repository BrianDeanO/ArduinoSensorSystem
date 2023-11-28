using Microsoft.EntityFrameworkCore;

namespace backEndApp.Models;

public class TemperatureItemContext : DbContext
{
    public TemperatureItemContext(DbContextOptions<TemperatureItemContext> options) : 
        base(options) {}

    public DbSet<TemperatureItem> TemperatureItems { get; set; } = null!;

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    //     optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=;Trusted_Connection=True;");
    // }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    //     optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=;Trusted_Connection=True;");
    // }

}

