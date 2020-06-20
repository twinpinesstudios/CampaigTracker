using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAccess
{
    public class CampaignContextDesignTimeFactory
        : IDesignTimeDbContextFactory<CampaignContext>
    {
        public CampaignContext CreateDbContext(string[] args)
        {
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var builder = new DbContextOptionsBuilder<CampaignContext>();
			var connectionString = configuration.GetConnectionString("CampaignMigrationConnection");
			builder.UseSqlServer(connectionString);

			return new CampaignContext(
				builder.Options
			);
		}
    }
}
