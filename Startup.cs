using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleAPIBootstrap;
using AutoMapper;


namespace SampleCoreAPI
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
            services.AddControllers();

            // Bootstrap the application
            services.ConfigureApplication(x =>
            {
                x.ConnectionString = Configuration["DatabaseConnectionString"];
                x.SqlServerActions = o => o.EnableRetryOnFailure(5);
            });

            //services
            //.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options => Configuration.GetSection("Authentication").Bind(options));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DefaultHttpPipeline(app);

        }


        private void DefaultHttpPipeline(IApplicationBuilder app)
        {
            app.UseRouting();
            //var origins = GetCorsOrigins();
            //app.UseCors(builder => builder
            //    .WithOrigins(origins)
            //    .SetPreflightMaxAge(TimeSpan.FromHours(24))
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

    //    private string[] GetCorsOrigins() => Configuration
    //.GetValue<string>("AllowedOrigins")
    //.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
