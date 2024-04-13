using Microsoft.EntityFrameworkCore;
using backEndApp;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using backEndApp.Repository;
using backEndApp.Hubs;
using System.Text.Json.Serialization;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var databaseMode = Environment.GetEnvironmentVariable("APPLICATION_DATABASE_MODE");
if(databaseMode != null && databaseMode == "InMemory") {
    builder.Services.AddScoped<IDeviceRepository, LocalDeviceRepository>();
    builder.Services.AddScoped<IUserRepository, LocalUserRepository>();
    builder.Services.AddScoped<ISensorRepository, LocalSensorRepository>();
    builder.Services.AddScoped<ISensorDataRepository, LocalSensorDataRepository>();
    builder.Services.AddScoped<ISensorConfigRepository, LocalSensorConfigRepository>();
    builder.Services.AddScoped<IUserDeviceRepository, LocalUserDeviceRepository>();
}
else {
    builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ISensorRepository, SensorRepository>();
    builder.Services.AddScoped<ISensorDataRepository, SensorDataRepository>();
    builder.Services.AddScoped<ISensorConfigRepository, SensorConfigRepository>();
    builder.Services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
}

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  => {
            policy.WithOrigins("http://localhost:3000");
        });
});

builder.Services.AddDbContext<SensorSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FrontendAppCon")));

builder.Services.AddDistributedMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app) {
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    if(scopedFactory != null) {
        using (var scope = scopedFactory.CreateScope()) {
            var service = scope.ServiceProvider.GetService<Seed>();
            if(service != null) {
                service.SeedSensorSystemContext();
            }
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => true));

app.UseAuthorization();

app.MapControllers();
app.MapHub<RealTimeDataHub>("realTimeDataHub");

app.Run();
