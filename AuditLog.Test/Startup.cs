using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditLog.Test
{

    /// <summary>
    /// კონგიგურაცია-საამქრო კლასი
    /// </summary>
    internal class Startup
    {

        /// <summary>
        /// კონსტრუქტორი
        /// </summary>
        /// <param name="configuration">კონფიგურაცია</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// კონფიგურაცია
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// სერვისების კონფიგურაცია
        /// </summary>
        /// <param name="services">სერვისები</param>
        public void ConfigureServices(IServiceCollection services)
        {
            //
            services.AddOptions();
            // http კონტექსტის ნაწილი
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // ელასტიკის ნაწილი
            var econfig = Configuration.GetSection("Elastic");
            services.Configure<Elastic.AuditLoggerConfiguration>(econfig);
            services.AddScoped<Elastic.AuditLogger>();
            // მონგოს ნაწილი
            var mconfig = Configuration.GetSection("Mongo");
            services.Configure<Mongo.AuditLoggerConfiguration>(mconfig);
            services.AddScoped<Mongo.AuditLogger>();
            // ვებ mvc ნაწილი
            services.AddMvc(options => options.EnableEndpointRouting = false);
            // swager ნაწილი
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration.GetValue<string>("Config:Version"), new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = Configuration.GetValue<string>("Config:Name"),
                    Version = Configuration.GetValue<string>("Config:Version")
                });
            });
        }

        /// <summary>
        /// კონფიგურაცია
        /// </summary>
        /// <param name="app">აპლიკაცია</param>
        /// <param name="env">ჰოსთინგი</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // დეველოპმენტ-კონფიგურაცია
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // ვებ mvc ნაწილი
            app.UseRouting();
            app.UseMvc();
            // swager ნაწილი
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    "/swagger/" + Configuration.GetValue<string>("Config:Version") + "/swagger.json",
                    Configuration.GetValue<string>("Config:Name")
                );
            });
        }

    }

}
