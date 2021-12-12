using MediatR;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;

namespace MicroRabbit.Transfer.Api
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

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Transfer Microservice",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "ROÇ",
                        Email = "okan.cilingiroglu@gmail.com"
                    },
                    Description = "A fully-featured banking microservice."
                });
            });     

            services.AddMediatR(typeof(Startup));

            RegisterServices(services);

            services.AddDbContext<TransferDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("TransferDbConnection"), x => x.MigrationsAssembly("MicroRabbit.Transfer.Api"));
            });           
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Defines a class that provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="environment">Provides information about the web hosting environment an application is running in.</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservice v1");
            });
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            ConfigueEventBus(app);
        }

        private static void ConfigueEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
        }
    }
}