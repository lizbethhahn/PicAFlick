using Microsoft.EntityFrameworkCore;
using PicAFlick.Data.Context;
using PicAFlick.Data.Repositories;
using PicAFlick.Infrastructure.Tmdb;
using PicAFlick.Services.Implementations;
using PicAFlick.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Load the TMDb API token from .NET User Secrets for local development.
var tmdbApiToken = builder.Configuration["Tmdb:ApiToken"]
    ?? throw new InvalidOperationException("TMDb API token is missing.");

builder.Services.AddHttpClient<ITmdbApiClient, TmdbApiClient>();
builder.Services.AddControllers();

// Load the local SQL Server connection string from appsettings.Development.json.
var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Database connection string is missing.");

// Add services to the container.
builder.Services.AddDbContext<WatchlistContext>(opt =>
{
    opt.UseSqlServer(connectionString);
    if (builder.Environment.IsDevelopment())
    {
        opt.EnableSensitiveDataLogging();       // dev-only
        opt.EnableDetailedErrors();
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
    builder.Services.AddHttpClient<ITmdbApiClient, TmdbApiClient>(client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tmdbApiToken}");
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();

builder.Services.AddHttpsRedirection(o => o.HttpsPort = 7043);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularOrigin");
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();