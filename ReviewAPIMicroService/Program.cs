using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common;

namespace ReviewAPIMicroService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args)
                .UseKestrel(c => c.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //Below lines take care of local debugging, the configuration file is at the Solution level
                    if (hostingContext.HostingEnvironment.IsLocalhost())
                    {
                        var env = hostingContext.HostingEnvironment;
                        var configFolder = Path.Combine(env.ContentRootPath, "..");
                        config.AddJsonFile(Path.Combine(configFolder,
                            $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json"), true);
                    }

                    //Below line takes care of Deployment scenarios - Path for configuration file will be determined at runtime using Environment Variable
                    config.AddJsonFile(Environment.GetEnvironmentVariable("CONFIGURATION_PATH") +
                                       $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true);
                })
                .ConfigureAppConfiguration(ic => ic.AddJsonFile(Environment.GetEnvironmentVariable("SECRET_PATH") + "secrets.json", true))
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            CreateWebHostBuilder(args).Build();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
