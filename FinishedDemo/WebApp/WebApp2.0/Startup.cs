using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApp2._0.Middleware;
using Microsoft.AspNetCore.Http;
using WebApp2._0.Services;

namespace WebApp2._0
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
            services.AddMvc();

            //services.AddScoped<IDataService, DataService>();
            //services.AddTransient<IDataService, DataService>();
            //services.AddSingleton<IDataService, DataService>();
            services.AddScoped<IDataService>(sp => new DataService());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseEncodeUri();

            // Show custom middleware inline
            app.Use(async (context, next) => {

                await next();

                string redirect = context.Response.Headers["X-Redirect"];
                if (!string.IsNullOrWhiteSpace(redirect)) {
                    context.Response.Headers.Add("X-nonsense", "pure nonsense");
                }
            });

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/Home/Contact"), (app2) => {
                app2.Use(async (context, next) => {

                    context.Response.Headers.Add("X-ContactUs", "Contact Page");
                    await next();
                });
            });

            app.Map("/Home/About", (app2) => {
                app2.Use(async (context, next) => {

                    context.Response.Headers.Add("X-onlyAbout", "this the about page");
                    await next();
                });
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Basic middleware showing Run statement
            //app.Use(async (context, next) => {

            //    Console.Write("Why am I here?\n");

            //    await next.Invoke();

            //    Console.Write("After here?\n");

            //    //Don't write to the response after calling the next delegate

            //});

            //app.Run(async (context) => {
            //    Console.Write("Hello ILM!\n");
            //    await context.Response.WriteAsync("<p>Hello ILM!</p>");
            //});
        }
    }
}
