using CampaignTracker.API.Setup.Configs;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using System;

namespace CampaignTracker.API.Setup
{
    public class CampaignTrackerODataModelBuilder
    {

        public static VersionedODataModelBuilder GenerateModelBuilder(IServiceProvider serviceProvider)
        {
            var options = Options.Create(new ApiVersioningOptions
            {
                DefaultApiVersion = ODataApiVersions.Initial,
                AssumeDefaultVersionWhenUnspecified = true,
                UseApiBehavior = true
            });

            var svc = serviceProvider.GetService(typeof(IActionDescriptorCollectionProvider)) as IActionDescriptorCollectionProvider;

            var modelBuilder = new VersionedODataModelBuilder(svc, options)
            {
                ModelBuilderFactory = () =>
                {
                    var convBuilder = new ODataConventionModelBuilder();
                    convBuilder.EnableLowerCamelCase();
                    return convBuilder;
                }
            };

            modelBuilder.ModelConfigurations.Add(new FactionModelConfiguration());

            return modelBuilder;
        }
    }
}
