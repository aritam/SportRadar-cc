using SportRadar.API.Configuration;
using SportRadar.API.Services;
using SportRadar.API.Services.Interfaces;
using SportRadar.API.Utilities;
using SportRadar.API.Utilities.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<ITeamDataService, DefaultTeamDataService>();
builder.Services.AddTransient<IPlayerDataService, DefaultPlayerDataService>();
builder.Services.AddHttpClient<ITeamDataService, DefaultTeamDataService>();
builder.Services.AddHttpClient<IPlayerDataService, DefaultPlayerDataService>();
builder.Services.AddTransient<ISerializer, CsvSerializer>();

// Register pre-configured instance of ApiOptions class

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("ApiOptions"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
