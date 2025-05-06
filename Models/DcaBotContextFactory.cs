using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Models.Scaffolded;

namespace Models;

public class DcaBotContextFactory : IDesignTimeDbContextFactory<DcaBotContext>
{

    public DcaBotContext CreateDbContext() => CreateDbContext([]);

    public DcaBotContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var dbName = Environment.GetEnvironmentVariable("TARGET_DB");

        // Fallback database names depending on environment
        if (string.IsNullOrEmpty(dbName))
        {
            dbName = environment switch
            {
                "Development" => "dca_bot",
                /*"Testing" => "dca_bot_test",
                "Staging" => "dca_bot_staging",
                "Production" => "dca_bot_prod",*/
                _ => "dca_bot" // default fallback
            };
        }

        var connectionString = $"server=localhost;database={dbName};user=user;password=password;";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 41));

        var optionsBuilder = new DbContextOptionsBuilder<DcaBotContext>();
        optionsBuilder.UseMySql(connectionString, serverVersion)
                      .EnableDetailedErrors()
                      .EnableSensitiveDataLogging();

        Console.WriteLine($"[DcaBotContextFactory] Connecting to DB '{dbName}' under Environment '{environment}'");

        return new DcaBotContext(optionsBuilder.Options);
    }

}