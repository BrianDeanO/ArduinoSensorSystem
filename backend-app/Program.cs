using Microsoft.EntityFrameworkCore;
using backEndApp.Models;
// using backEndApp.Data;


var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  => {
            policy.WithOrigins("http://localhost:3000");
        });
});

builder.Services.AddDbContext<SensorSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HecateAppCon")));

builder.Services.AddDistributedMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// if(args.Length == 1 && args[0].ToLower() == "seeddata") {
//     Seed.SeedData(app);
// }

// Seed.SeedData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseDefaultFiles();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
