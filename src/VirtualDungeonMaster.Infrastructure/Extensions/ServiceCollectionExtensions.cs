using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using VirtualDungeonMaster.Domain.Adventures.Repositories;
using VirtualDungeonMaster.Domain.AI;
using VirtualDungeonMaster.Domain.Characters.Repoositories;
using VirtualDungeonMaster.Infrastructure.AI;
using VirtualDungeonMaster.Infrastructure.Mapping;
using VirtualDungeonMaster.Infrastructure.Persistance;
using VirtualDungeonMaster.Infrastructure.Persistance.Repositories;

namespace VirtualDungeonMaster.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Retrieve connection string from configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Register Entity Framework Core for PostgreSQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Register AutoMapper with all profiles in this assembly
            services.AddAutoMapper(typeof(EntityToDomainProfile).Assembly);

            // Get Ollama modelId and endpoint from configuration (with defaults)
            var modelId = configuration["Ollama:ModelId"] ?? "llama3";
            var endpointStr = configuration["Ollama:Endpoint"] ?? "http://localhost:11434";
            var endpoint = new Uri(endpointStr);

            // Register Semantic Kernel Ollama connector
#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            services.AddOllamaChatCompletion(
                modelId: modelId,
                endpoint: endpoint
            );
#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            // Register repositories
            services.AddTransient<IAdventuresRepository, AdventuresRepository>();
            services.AddTransient<ICharactersRepository, CharactersRepository>();

            IKernelBuilder kernelBuilder = services.AddKernel();
            
            // Register services
            services.AddTransient<IAiService, SemanticKernelAiService>();

            return services;
        }
    }
}
