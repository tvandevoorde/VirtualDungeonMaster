using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtualDungeonMaster.Application.Adventures;
using VirtualDungeonMaster.Application.Characters;
using VirtualDungeonMaster.Domain.Adventures.Services;
using VirtualDungeonMaster.Domain.Characters.Services;
using VirtualDungeonMaster.Infrastructure.Extensions;

namespace VirtualDungeonMaster.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVirtualDungeonMasterApp(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInfrastructureServices(configuration);
            services.AddTransient<IAdventureService, AdventureService>();
            services.AddTransient<ICharacterService, CharacterService>();
            return services;
        }
    }
}
