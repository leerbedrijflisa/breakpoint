using Lisa.Breakpoint.WebApi.database;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Raven.Client;
using Raven.Client.Document;

namespace Lisa.Breakpoint.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.ConfigureCors(options =>
            {
                options.AddPolicy("Breakpoint", builder => 
                {
                    builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddInstance<IDocumentStore>(new DocumentStore() { Url = "http://localhost:8080", DefaultDatabase = "breakpoint" });
            services.AddSingleton<RavenDB>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("Breakpoint");
            app.UseMvcWithDefaultRoute();
        }
    }
}