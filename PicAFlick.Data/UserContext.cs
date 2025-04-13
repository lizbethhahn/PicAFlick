using Microsoft.EntityFrameworkCore;

using PicAFlick.Domain.Entities;

namespace PicAFlick.Data
{
    public class UserContext : DbContext
    {   
        public DbSet<UserMovie> UserMovies { get; set; }
        public DbSet<UserTVShow> UsersTVShows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source= ECHO;Database=PicAFlickData;Trusted_Connection=True;TrustServerCertificate=True", options => options.MaxBatchSize(100));
        }
    }
}
