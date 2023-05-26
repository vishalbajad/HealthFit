using HealthFit.API_Connector;
using HealthFit.Object_Provider.Model;
using HealthFit_Web.CustomAttributes;

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
            services.AddRazorPages();
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
