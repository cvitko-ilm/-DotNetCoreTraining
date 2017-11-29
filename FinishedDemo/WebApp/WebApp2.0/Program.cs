using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApp2._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    // add in custom setting file that will override the default appsettings.json file
                    IHostingEnvironment env = builderContext.HostingEnvironment;

                    config.AddJsonFile("configSettings.json", optional: false);
                })
                .UseStartup<Startup>()
                .Build();
    }
}
