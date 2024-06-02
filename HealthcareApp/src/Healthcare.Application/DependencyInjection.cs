using System.Reflection;
using Application.Abstractions;
using Application.Implementations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace Healthcare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddScoped<IUserAuthenticationService, AuthenticationService>();

        return services;
    }
}