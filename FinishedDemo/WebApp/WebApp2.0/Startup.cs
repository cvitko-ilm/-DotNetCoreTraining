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
using WebApp2._0.Models;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.IO;

namespace WebApp2._0
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _hostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddSessionStateTempDataProvider();

            //services.AddScoped<IDataService, DataService>();
            //services.AddTransient<IDataService, DataService>();
            //services.AddSingleton<IDataService, DataService>();
            services.AddScoped<IDataService>(sp => new DataService());

            //Get configuration setting setup
            services.Configure<DataSettings>(Configuration.GetSection("DataSettings"));

            // routing
            services.AddRouting();

            //file providers
            var physicalProvider = _hostingEnvironment.ContentRootFileProvider;
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly());
            var compositeProvider = new CompositeFileProvider(physicalProvider, embeddedProvider);

            // choose one provider to use for the app and register it
            //services.AddSingleton<IFileProvider>(physicalProvider);
            //services.AddSingleton<IFileProvider>(embeddedProvider);
            services.AddSingleton<IFileProvider>(compositeProvider);

            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            configuration.DisableTelemetry = true;

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions() {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files/images")),
                RequestPath = new PathString("/StaticFiles")
            });

            app.UseSession();

            app.UseEncodeUri();

            // Show custom middleware inline
            app.Use(async (context, next) => {
                
                await next();

                string redirect = context.Response.Headers["X-Redirect"];
                if (!string.IsNullOrWhiteSpace(redirect)) {
                    //context.Response.Headers.Add("X-nonsense", "pure nonsense");
                    Debug.WriteLine($"***** X-Redirect found value: {redirect}");
                }
            });

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/Home/Contact"), (app2) => {
                app2.Use(async (context, next) => {

                    Debug.WriteLine($"***** Contact Page");
                    await next();
                });
            });

            app.Map("/Home/About", (app2) => {
                app2.Use(async (context, next) => {

                    Debug.WriteLine($"***** About Page");
                    await next();
                });
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //custom routing
            app.UseRouter(routes => {
                routes.MapGet("test/{id:int}", context => {
                    var id = context.GetRouteValue("id");
                    return context.Response.WriteAsync($"Hi, number: {id}");
                });
                routes.MapGet("test/{id:alpha}", context => {
                    var id = context.GetRouteValue("id");
                    return context.Response.WriteAsync($"Hi, string: {id}");
                });
                routes.MapGet("test/{*slug}", context => {
                    var id = context.GetRouteValue("id");
                    return context.Response.WriteAsync("Slugs!");
                });
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
