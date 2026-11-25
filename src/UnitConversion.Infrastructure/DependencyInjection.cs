using Microsoft.Extensions.DependencyInjection;
using UnitConversion.Application.Interfaces;
using UnitConversion.Infrastructure.ConversionStrategies;
using UnitConversion.Infrastructure.Providers;

namespace UnitConversion.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IUnitProvider, InMemoryUnitProvider>();
        services.AddSingleton<IConversionStrategyFactory, ConversionStrategyFactory>();
        return services;
    }
}