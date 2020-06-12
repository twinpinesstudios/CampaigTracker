using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CampaignTracker.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((ctx, conf) =>
                    {
                        conf.SetBasePath(ctx.HostingEnvironment.ContentRootPath);
                    });

                    webBuilder
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<Startup>();
                });
    }
}
