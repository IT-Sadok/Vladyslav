using Microsoft.Extensions.DependencyInjection;

namespace Healthcare.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        return services;
    }
}