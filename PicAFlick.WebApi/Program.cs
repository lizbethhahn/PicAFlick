using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PicAFlick.Data.Context;
using PicAFlick.Data.Repositories;
using PicAFlick.Infrastructure.Tmdb;
using PicAFlick.Services.Implementations;
using PicAFlick.Services.Interfaces;
using System;

var envPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, ".env");
               Env.Load(envPath);   
var tmdbApiToken = Environment.GetEnvironmentVariable("TMDB_API_TOKEN");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<ITmdbApiClient, TmdbApiClient>();
builder.Services.AddControllers();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("Default"); ;
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Missing DB connection. Set DB_CONNECTION_STRING or ConnectionStrings:Default.");

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