using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Localisation.Properties;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using CampaignTracker.API;
using CampaignTracker.API.Setup;

namespace CampaignTracker.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
			AddOdataWithVersioning(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

			app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

			app.UseHttpsRedirection();

			var modelBuilder = CampaignTrackerODataModelBuilder.GenerateModelBuilder(app.ApplicationServices);

			app.UseMvc(routes =>
			{
				routes
					.Count()
					.Select()
					.Filter()
					.Expand()
					.OrderBy()
					.SkipToken();

				routes.EnableDependencyInjection();
				routes.MapVersionedODataRoutes("odata", "v{version:apiVersion}", modelBuilder.GetEdmModels());
			});

			app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

		public void AddOdataWithVersioning(IServiceCollection services)
		{
			services
				.AddControllers(x => x.EnableEndpointRouting = false)
				//.AddApplicationPart(typeof(AssetController).Assembly)
				.AddDataAnnotationsLocalization(o => o.DataAnnotationLocalizerProvider = (type, factory) =>
				{
					var assemblyName = new AssemblyName(typeof(LocalStrings).GetTypeInfo().Assembly.FullName);
					return factory.Create("LocalStrings", assemblyName.Name);
				})
				.AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.Converters.Add(new StringEnumConverter());
					options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
					options.SerializerSettings.MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Ignore;
					options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
					options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
					options.SerializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
					options.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
					options.SerializerSettings.StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.Default;
					options.SerializerSettings.ConstructorHandling = Newtonsoft.Json.ConstructorHandling.Default;
					options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
					options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;
					options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
				});

			services.AddODataApiExplorer(options =>
			{
				options.GroupNameFormat = "VVV";
				options.SubstituteApiVersionInUrl = true;
				options.DefaultApiVersion = ODataApiVersions.Initial;
			});

			services.AddVersionedApiExplorer(options =>
			{
				options.GroupNameFormat = "VVV";
				options.SubstituteApiVersionInUrl = true;
				options.DefaultApiVersion = ODataApiVersions.Initial;
			});

			services.AddApiVersioning(opt =>
			{
				opt.AssumeDefaultVersionWhenUnspecified = true;
				opt.DefaultApiVersion = ODataApiVersions.Initial;
				opt.ApiVersionReader = new UrlSegmentApiVersionReader();
			});

			services.AddOData()
				.EnableApiVersioning();
		}
	}
}
