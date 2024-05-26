using Application.DTOs.Login;
using Application.DTOs.Register;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Healthcare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // var assembly = typeof(DependencyInjection).Assembly;
        // services.AddMediatR(conf =>
        //     conf.RegisterServicesFromAssembly(assembly));
        //
        // services.AddValidatorsFromAssemblyContaining<RegisterUserDTOValidator>();
        
        var assembly = typeof(RegisterUserDTOValidator).Assembly; 
        services.AddMediatR(conf =>
            conf.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssemblyContaining<RegisterUserDTOValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginUserDTOValidator>();
        
        return services;
    }
}