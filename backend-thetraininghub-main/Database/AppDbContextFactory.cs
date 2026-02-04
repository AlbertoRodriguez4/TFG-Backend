using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using AA2_CS.Database;
namespace AA2_CS.Database;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var connectionString = "Host=localhost;Port=3786;Database=postgres;Username=postgres;Password=password";
        optionsBuilder.UseNpgsql(connectionString);
        return new AppDbContext(optionsBuilder.Options);
    }
}