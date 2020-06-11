using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CampaignContext
        : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //This is where to ef core configs go

            base.OnModelCreating(builder);
        }
    }
}
