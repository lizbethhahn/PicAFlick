using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CapstoneChatbot.App.Data
{
    public class CapstoneDbContextFactory :  IDesignTimeDbContextFactory<CapstoneDbContext>
    {
        public CapstoneDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CapstoneDbContext>();
            var dbPath = Path.Combine(AppContext.BaseDirectory, "capstone.db");
            var connectionString = $"Data Source={dbPath}";
            optionsBuilder.UseSqlite(connectionString);
            return new CapstoneDbContext(optionsBuilder.Options);
        }
    }
}
