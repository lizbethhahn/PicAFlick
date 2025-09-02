using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PicAFlick.Data.Context;
using PicAFlick.Infrastructure.Tmdb;

var envPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, ".env");
               Env.Load(envPath);
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
var tmdbApiToken = Environment.GetEnvironmentVariable("TMDB_API_TOKEN");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Missing environment variables.");
    return;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<WatchlistContext>(options => options
                .UseSqlServer(connectionString));
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();