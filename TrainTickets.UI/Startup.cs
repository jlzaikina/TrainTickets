using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TrainTickets.Infrastructure;

using TrainTickets.Infrastructure.Adapters.Postgres;
using TrainTickets.UI.Application.Test.Handlers;
using TrainTickets.UI.Application.Test.Mappers;
using TrainTickets.UI.Ports;
using TrainTickets.Core.Settings;

namespace TrainTickets.UI;

public class Startup
{
    public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
                // Configuration
             var connectionString = Configuration["CONNECTION_STRING"];
            
            services.AddDbContext<ApplicationDbContext>((sp,optionsBuilder) =>
                {
                    optionsBuilder.UseNpgsql(connectionString);
                }
            );

        //Регистрация сервисов
        services.AddTransient<IUserHandler, UserHandler>();
             services.AddTransient<IUserMapper, UserMapper>();
             services.AddTransient<IPassMapper, PassMapper>();
        services.AddTransient<ITicketMapper, TicketMapper>();
        services.AddTransient<ITrainMapper, TrainMapper>();
        services.AddTransient<IPassengerHandler, PassHandler>();
             services.AddTransient<ITicketHandler, TicketHandler>();
        services.AddTransient<ITrainHandler, TrainHandler>();
        services.AddTransient<IUserRepository, UserPostgresRepository>();
             services.AddTransient<IPassRepository, PassPostgresRepository>();
             services.AddTransient<ITicketRepository, TicketPostgresRepository>();
        services.AddTransient<ITrainRepository, TrainPostgresRepository>();

        services.AddTransient<IScheduleHandler, ScheduleHandler>();
            services.AddTransient<IScheduleMapper, ScheduleMapper>();
            services.AddTransient<IScheduleRepository, SchedulePostgresRepository>();
            services.AddTransient<IPassRepository, PassPostgresRepository>();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);



        services.AddControllers();
           
            //Swagger
             services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new OpenApiInfo
                 {
                     Title = "Сервис бронирования билетов",
                     Description = "Сервис бронирования билетов (ASP.NET Core 8.0)",
                 });
             });
             services.AddControllers();
             services.AddEndpointsApiExplorer();
             services.AddHealthChecks();
            
             services.AddCors(options =>
             {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // Разрешить запросы с любого источника
                        .AllowAnyMethod() // Разрешить все HTTP-методы (GET, POST и т.д.)
                        .AllowAnyHeader(); // Разрешить все заголовки
                });
             });

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Сервис бронирования билетов");
                options.RoutePrefix = string.Empty;
            });
            app.UseHealthChecks("/health");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
}