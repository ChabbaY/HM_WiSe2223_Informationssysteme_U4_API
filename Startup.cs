using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddMvc();

            services.AddDbContext<Context>(options => {
                options.UseInMemoryDatabase("InMemoryDb");
            });

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v0", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v0" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Context context) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v0/swagger.json", "My API V0");
            });

            //Initialize Data
            if(context.Customers.Where(c => c.Id == 1).FirstOrDefault() == null) {
                context.Customers.Add(new DataObject.Customer() { Name = "Test1" });
                context.Customers.Add(new DataObject.Customer() { Name = "Test2" });
                context.Customers.Add(new DataObject.Customer() { Name = "Test3" });
                context.Customers.Add(new DataObject.Customer() { Name = "Test4" });
                context.Customers.Add(new DataObject.Customer() { Name = "Test5" });
                context.Customers.Add(new DataObject.Customer() { Name = "Test6" });
                context.SaveChanges();
            }


            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
