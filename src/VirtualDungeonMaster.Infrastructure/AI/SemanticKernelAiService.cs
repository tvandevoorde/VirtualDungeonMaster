using System.Reflection;
using Microsoft.SemanticKernel;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.AI;
using VirtualDungeonMaster.Domain.Characters;

namespace VirtualDungeonMaster.Infrastructure.AI
{
    public class SemanticKernelAiService(Kernel kernel) : IAiService
    {
        private readonly Kernel _kernel = kernel;
        private readonly string _basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        public Task<string> GenerateAdventureIntroAsync(Character character, string? title, CancellationToken cancellationToken = default)
        {
            var arguments = new KernelArguments
            {
                { "adventureTitle", title ?? string.Empty },
                { "characterName", character.Name },
                { "characterRace", character.Race },
                { "characterClass", character.Class },
                { "characterBackground", character.Background },
            };
            KernelFunction function = KernelFunctionFactory.CreateFromPrompt(File.ReadAllText($"{_basePath}/AI/Prompts/DungeonMaster/GenerateAdventureIntroPrompt.txt"));
            return _kernel.InvokeAsync(function, arguments, cancellationToken)
                .ContinueWith(task => task.Result.ToString() ?? string.Empty, cancellationToken);
        }

        public Task<string> GenerateRecapAsync(AdventureSession session, CancellationToken cancellationToken = default)
        {
            var arguments = new KernelArguments
            {
                { "sessionSummary", string.Join("\n", session.Events.Select(e => $"Player: {e.PlayerInput}\nDM: {e.AIResponse}")) }
            };
            KernelFunction function = KernelFunctionFactory.CreateFromPrompt(File.ReadAllText($"{_basePath}/AI/Prompts/DungeonMaster/GenerateRecapPrompt.txt"));

            return _kernel.InvokeAsync(function, arguments, cancellationToken)
                .ContinueWith(task => task.Result.ToString() ?? string.Empty, cancellationToken);
        }

        public Task<string> GenerateTurnResponseAsync(AdventureSession session, string playerInput, CancellationToken cancellationToken = default)
        {
            var arguments = new KernelArguments
            {
                { "sessionSummary", string.Join("\n", session.Events.Select(e => $"Player: {e.PlayerInput}\nDM: {e.AIResponse}")) },
                { "playerInput", playerInput }
            };
            KernelFunction function = KernelFunctionFactory.CreateFromPrompt(File.ReadAllText($"{_basePath}/AI/Prompts/DungeonMaster/GenerateTurnResponsePrompt.txt"));
            return _kernel.InvokeAsync(function, arguments, cancellationToken)
                .ContinueWith(task => task.Result.ToString() ?? string.Empty, cancellationToken);
        }
    }
}
