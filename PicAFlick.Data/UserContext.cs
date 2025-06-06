using DotNetEnv;
using Microsoft.EntityFrameworkCore;

using PicAFlick.Domain.Entities;

namespace PicAFlick.Data
{
    public class UserContext : DbContext
    {
        public DbSet<UserMovie> UserMovies { get; set; }
        public DbSet<UserTVShow> UsersTVShows { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        Env.Load();

        //        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        //            ?? throw new InvalidOperationException("DB_CONNECTION_STRING");

        //        optionsBuilder.UseSqlServer(
        //        connectionString, options => options.MaxBatchSize(100));
        //    }
        //}
    }
}