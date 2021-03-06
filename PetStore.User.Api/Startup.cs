using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PetStore.User.Api.Infrastructure;
using Prometheus;

namespace PetStore.User.Api
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
            services.AddLamar(new ApiRegistry());

            if (Program.Debug)
            {
                services.AddLogging(x =>
                {
                    x.AddDebug();
                    x.AddConsole();
                });
            }
            
            services.AddSwaggerGen(c =>
            {
                c.TagActionsBy(p => new List<string> {"User - Operations about user"});
                c.SwaggerDoc("swagger", new OpenApiInfo { Title = "User API", Version = "v1" });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (Program.Debug || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(x => x.RouteTemplate = "/{documentName}.json");
            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = "";
                x.SwaggerEndpoint("/swagger.json", "User Api v1");
                x.ConfigObject.DefaultModelsExpandDepth = -1;
            });

            app.UseRouting();
            app.UseHttpMetrics();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}
