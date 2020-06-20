using Factions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace CampaignTracker.API.Setup.Configs
{
    public class FactionModelConfiguration
        : BaseODataModelConfiguration
    {
        public override void Apply(ODataModelBuilder builder, ApiVersion apiVersion) => this.ConfigureDefault(builder);

        private EntityTypeConfiguration<Faction> ConfigureDefault(ODataModelBuilder builder)
        {
            var faction = this.ConfigureEntitySet<Faction>(builder).EntityType;

            faction
                .HasKey(x => x.Id)
                .Property(x => x.Id)
                .Name = "id";

            return faction;
        }
    }
}
