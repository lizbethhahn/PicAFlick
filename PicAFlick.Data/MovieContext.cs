using Microsoft.EntityFrameworkCore;

using PicAFlick.Domain.Entities;

namespace PicAFlick.Data
{
    public class MovieContext : DbContext
    {   
        public DbSet<Movie> Movies { get; set; }
        public DbSet<TVShow> TVShows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source= ECHO;Database=PicAFlickData;Trusted_Connection=True;TrustServerCertificate=True", options => options.MaxBatchSize(100));
        }
    }
}
