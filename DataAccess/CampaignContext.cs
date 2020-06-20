using DataAccess.Configs;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CampaignContext
        : DbContext
    {

        public CampaignContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //This is where to ef core configs go
            builder.ApplyConfiguration(new FactionEntityConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
