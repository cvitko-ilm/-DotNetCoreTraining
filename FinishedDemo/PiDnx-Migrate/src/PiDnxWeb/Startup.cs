using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace PiDnxWeb
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole()
                .AddDebug();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";

                await PlatformMessage.Messages.ForWeb(context.Response.Body);
            });
        }
    }
}
