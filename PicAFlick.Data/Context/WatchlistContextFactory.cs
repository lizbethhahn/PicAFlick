using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PicAFlick.Data.Context
{
    public class WatchlistContextFactory : IDesignTimeDbContextFactory<WatchlistContext>
    {
        public WatchlistContext CreateDbContext(string[] args)
        {
            var connString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                             ?? throw new InvalidOperationException(
                                  "Set DB_CONNECTION_STRING in your environment");

            var options = new DbContextOptionsBuilder<WatchlistContext>()
                .UseSqlServer(connString)
                .Options;

            return new WatchlistContext(options);
        }
    }
}