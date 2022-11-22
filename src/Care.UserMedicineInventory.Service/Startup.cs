using Care.Common.MassTransit;
using Care.Common.MongoDB;
using Care.Common.Settings;
using Care.UserMedicineInventory.Service.Interfaces;
using Care.UserMedicineInventory.Service.Models;
using Care.UserMedicineInventory.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Care.UserMedicineInventory.Service
{
    public class Startup
    {
        // AllowedOrigin = is locatec in appsettings.Development to remove Cors
        private const string AllowedOriginDevelopmentSetting = "AllowedOrigin";
        private const string AllowedOriginSetting = "AllowedHosts";
        private ServiceSettings serviceSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            services.AddMongo()
                    .AddRepository<UserMedicineInventoryItem>("usermedicineinventoryitems")
                    .AddRepository<MedicineItem>("medicineitems")
                    .AddMassTransitWithRabbitMQ();

            services.AddSingleton<IUserMedicineInventoryService, UserMedicineInventoryService>();

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Care.UserMedicineInventory.Service", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Care.UserMedicineInventory.Service v1"));

                app.UseCors(builder =>
                {
                    builder.WithOrigins(Configuration[AllowedOriginDevelopmentSetting])
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            }

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Care.UserMedicineInventory.Service v1"));
            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration[AllowedOriginSetting])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
            });

            app.UseAuthorization();
            // app.UseDeveloperExceptionPage();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
