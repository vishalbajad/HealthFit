using HealthFit.CustomAttributes;
using HealthFit_APIs.Model;
using Microsoft.AspNetCore.Mvc;

namespace HealthFit_APIs
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers(options =>
            {
                options.Filters.Add<CustomExceptionFilter>();
            });

            services.AddDistributedMemoryCache();

            services.AddMvc(options =>
            {
                options.Filters.Add(new CustomRequireHttpsAttribute());
            });

            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddAntiforgery(o => o.HeaderName = "30ee3956632c35d9840037633e0110b6");
            services.Configure<AppSettingsConfigurations>(Configuration);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHsts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
