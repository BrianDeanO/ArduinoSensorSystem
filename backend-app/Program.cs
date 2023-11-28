using Microsoft.EntityFrameworkCore;
using backEndApp.Models;
using backEndApp.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();
builder.Services.AddControllersWithViews();


// builder.Services.AddDbContext<TemperatureItemContext>(opt =>
//     opt.UseInMemoryDatabase("TemperatureList"));
// builder.Services.AddDbContext<TodoContext>(opt =>
//     opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddDbContext<TemperatureItemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HecateAppCon")));
// builder.Services.AddDbContext<TodoContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("HecateAppCon")));

builder.Services.AddDistributedMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(args.Length == 1 && args[0].ToLower() == "seeddata") {
    Seed.SeedData(app);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
