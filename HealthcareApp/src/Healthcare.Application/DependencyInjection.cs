using System.Reflection;
using Application.Abstractions;
using Application.Implementations;
using FluentValidation;
using Healthcare.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Healthcare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddScoped<IUserAuthenticationService, AuthenticationService>();

        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        return services;
    }
}