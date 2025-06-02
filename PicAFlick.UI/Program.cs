using Domain.DTOs;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PicAFlick.Data;
using PicAFlick.Domain.Entities;
using PicAFlick.Domain.Services;
using System.Data;
using System.Text.Json;

namespace PicAFlick.UI
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            Env.Load();
            // Fetch API key from environment variable
            var apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY");
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Missing environment variables.");
                return;
            }

            var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Add DbContext
                services.AddDbContext<UserContext>(options =>
                    options.UseSqlServer(connectionString));

                // Register TmdbApiClient as ITmdbApiClient
                services.AddHttpClient<ITmdbApiClient, TmdbApiClient>();

                // Register App
                services.AddTransient<App>();
            })
            .Build();

            var app = host.Services.GetRequiredService<App>();
            await app.RunAsync();
        }
    }  
}