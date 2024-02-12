using Microsoft.EntityFrameworkCore;
using backEndApp;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using backEndApp.Repository;
using System.Text.Json.Serialization;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>();
builder.Services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  => {
            policy.WithOrigins("http://localhost:3000");
        });
});

builder.Services.AddDbContext<SensorSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HecateAppCon")));

// builder.Services.AddDbContext<SensorSystemContext>(options =>
//     options.UseSqlServer("Server=localhost,1433; Database=DockerDB;"));

builder.Services.AddDistributedMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app) {
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope()) {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedSensorSystemContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseDefaultFiles();

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
