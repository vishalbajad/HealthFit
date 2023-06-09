﻿using HealthFit.CustomAttributes;
using HealthFit_APIs.Model;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreRateLimit;
using HealthFit_APIs.CustomAttributes;
using Serilog.Events;
using Serilog;

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

            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddAntiforgery(o => o.HeaderName = "30ee3956632c35d9840037633e0110b6");
            services.Configure<AppSettingsConfigurations>(Configuration);
            // Set up Serilog
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
            app.UseIpRateLimiting();
            app.UseMiddleware<UploadRequestValidationMiddleware>();
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
