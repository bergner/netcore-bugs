using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RequestBuffering
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
            services.AddMvcCore(options => {
                options.InputFormatters.Add(new AtomEntryInputFormatter(new MvcOptions()));
                options.InputFormatters.Add(new XmlSerializerInputFormatter(new MvcOptions()));
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.Use(next => context => {
                Console.WriteLine("BODY IS OF TYPE: {0}", context.Request.Body.GetType());
                context.Request.EnableBuffering();
                //(new StreamReader(context.Request.Body)).ReadToEnd();
                //context.Request.Body.Position = 0;
                Console.WriteLine("BODY IS OF TYPE: {0}", context.Request.Body.GetType());
                return next(context);
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
