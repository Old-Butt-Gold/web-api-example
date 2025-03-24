using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace WebApiExample.ContextFactory;

/// <summary>
/// this class will help our application create a derived DbContext
/// instance during the design time which will help us with our migrations
/// </summary>
public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                b => b.MigrationsAssembly("WebApiExample"));
        // migration assembly is not in our main project, but in the Repository project.
        return new RepositoryContext(builder.Options);
    }
}