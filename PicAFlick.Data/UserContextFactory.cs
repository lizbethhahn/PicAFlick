using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PicAFlick.Data
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            var connString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                             ?? throw new InvalidOperationException(
                                  "Set DB_CONNECTION_STRING in your environment");

            var options = new DbContextOptionsBuilder<UserContext>()
                .UseSqlServer(connString)
                .Options;

            return new UserContext(options);
        }
    }
}
