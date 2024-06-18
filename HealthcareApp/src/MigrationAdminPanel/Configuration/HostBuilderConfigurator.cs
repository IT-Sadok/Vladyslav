using Application.Abstractions;
using Application.Abstractions.Decorators;
using Domain.Entities;
using Healthcare.Infrastructure.Persistance;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MigrationAdminPanel.Services;

namespace MigrationAdminPanel.Configuration;

public class HostBuilderConfigurator
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false);
                
                IConfiguration config = builder.Build();
                
                // Register DbContext with a connection string
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(config.GetConnectionString("HealthcareDb"));
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

                // Register Repository
                services.AddScoped<IUserManagerDecorator<ApplicationUser>, UserManagerDecorator<ApplicationUser>>();
                services.AddScoped<IAppointmentRepository, AppointmentRepository>();

                
                services.AddTransient<JsonMigrationsService>();
                services.AddTransient<XmlMigrationsService>();

                
                services.AddTransient<MyApp>();
            });
}