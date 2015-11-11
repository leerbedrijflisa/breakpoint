using Lisa.Breakpoint.WebApi.database;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.DependencyInjection;
using Newtonsoft.Json.Converters;
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
                //opts.SerializerSettings.Converters.Add(new StringEnumConverter());
            });
            services.AddCors(options =>
            {
                options.AddPolicy("Breakpoint", builder => 
                {
                    builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                });
            });
            var docStore = new DocumentStore() { Url = "http://localhost:8080", DefaultDatabase = "breakpoint" };
            docStore.Conventions.SaveEnumsAsIntegers = true;
            services.AddInstance<IDocumentStore>(docStore);
            services.AddSingleton<RavenDB>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIISPlatformHandler();
            app.UseCors("Breakpoint");
            app.UseMvcWithDefaultRoute();
        }
    }
}