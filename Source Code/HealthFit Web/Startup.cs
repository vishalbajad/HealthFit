using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Serilog;
using Serilog.Events;

namespace HealthFit_Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorPagesOptions(options => { options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()); });
            services.AddDistributedMemoryCache();
            services.AddScoped<HTTPConnector>();
            services.AddSession();
            services.Configure<SystemConfigurations>(Configuration);
            services.AddMvc(options =>
            {
                options.Filters.Add(new CustomRequireHttpsAttribute());
            });
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            // Add Serilog to the logging pipeline
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions { OnPrepareResponse = ctx => { ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600"); } });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseHsts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
