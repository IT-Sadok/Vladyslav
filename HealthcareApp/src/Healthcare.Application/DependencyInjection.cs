using Application.DTOs.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Healthcare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(RegisterUserDTOValidator).Assembly; 
        services.AddMediatR(conf =>
            conf.RegisterServicesFromAssembly(assembly));
        
        return services;
    }
}