using Microsoft.Extensions.DependencyInjection;
using VirtualDungeonMaster.Application.Adventures;
using VirtualDungeonMaster.Domain.Adventures;

namespace VirtualDungeonMaster.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAdventureService, AdventureService>();
            return services;
        }
    }
}
