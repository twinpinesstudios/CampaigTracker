using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Reflection;
using Localisation.Properties;

namespace CampaigTracker
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
            services.AddCors();

            services.AddRazorPages()
                .AddMvcOptions(opts =>
                {
                    opts.Filters.Add(new AuthorizeFilter());
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(o => o.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(LocalStrings).GetTypeInfo().Assembly.FullName);
                    return factory.Create("LocalStrings", assemblyName.Name);
                });

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(CreateCampaignContext)
                .InstancePerDependency()
                .As<DbContext>()
                .AsSelf();

            SetupRepositoryProvider(builder);
        }

        private void SetupRepositoryProvider(ContainerBuilder builder)
            => builder.Register(
                c =>
                {
                    CampaignContext campaignContext = null;

                    campaignContext = c.Resolve<CampaignContext>();
                    return new RepositoryProvider(campaignContext);
                    
                })
            .InstancePerDependency()
            .AsImplementedInterfaces()
            .AsSelf();


        private CampaignContext CreateCampaignContext(IComponentContext c)
        {
            CampaignContext context = null;

            try
            {
                var connectionString = "Server=tcp:campaigntracker.database.windows.net,1433;Initial Catalog=CampaignTracker-Local;Persist Security Info=False;User ID=user1;Password=HjWygwtg1yh;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                var dbOptionsBuilder = new DbContextOptionsBuilder()
                    .UseSqlServer(
                        connectionString,
                        opts =>
                        {
                            opts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                            opts.CommandTimeout(60);
                        }
                    )
                    .ConfigureWarnings(x => x.Ignore(CoreEventId.DetachedLazyLoadingWarning));

                context = new CampaignContext(dbOptionsBuilder.Options);
                context.Database.Migrate();
            }
            catch (ArgumentOutOfRangeException aex)
            {
                Trace.TraceError($"Argument out of range when configuration Context: {aex.Message}");
            }

            return context;
        }
    }
}
