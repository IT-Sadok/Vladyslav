using Application.Abstractions;
using Application.Implementations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Healthcare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
        
        services.AddScoped<IUserAuthenticationService, AuthenticationService>();

        return services;
    }
}