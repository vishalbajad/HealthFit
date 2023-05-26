using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

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
