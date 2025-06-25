using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using VirtualDungeonMaster.Domain.AI;
using VirtualDungeonMaster.Infrastructure.AI;
using VirtualDungeonMaster.Infrastructure.AI.Skills;
using VirtualDungeonMaster.Infrastructure.Mapping;
using VirtualDungeonMaster.Infrastructure.Persistance;

namespace VirtualDungeonMaster.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            // Register Entity Framework Core for PostgreSQL
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Register AutoMapper with all profiles in this assembly
            services.AddAutoMapper(typeof(EntityToDomainProfile).Assembly);

            // Register Semantic Kernel Ollama connector
#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            services.AddOllamaChatCompletion(
                modelId: "llama3",
                endpoint: new Uri("http://localhost:11434")
            );
#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            // Register Semantic Kernel AI service
            services.AddScoped<IAiService, SemanticKernelAiService>();

            services.AddSingleton(sp => KernelPluginFactory.CreateFromType<CharacterLookupSkill>());

            return services;
        }
    }
}
