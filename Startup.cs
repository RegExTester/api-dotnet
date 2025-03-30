using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegExTester.Api.DotNet.Services;
using System;

namespace RegExTester.Api.DotNet
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
            services.AddResponseCaching();
            services.AddRequestTimeouts(options =>
            {
                options.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(5) };
            });
            services.AddControllers();
            services.AddCors();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddTransient<IRegExProcessor, RegExProcessor>();
            services.AddSingleton<ITelemetryService>(
                new TelemetryService(Configuration["Cosmos:ConnectionString"], Configuration["Cosmos:Database"], Configuration["Cosmos:Container"])
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(
                builder => builder
                    .WithOrigins(Configuration.GetSection("AllowCors").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    #if DEBUG
                    .AllowAnyOrigin()
                    #endif
            );
            app.UseResponseCaching();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseRequestTimeouts();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
