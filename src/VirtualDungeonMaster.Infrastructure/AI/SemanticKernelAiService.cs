using Microsoft.SemanticKernel;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.AI;

namespace VirtualDungeonMaster.Infrastructure.AI
{
    public class SemanticKernelAiService(Kernel kernel) : IAiService
    {
        private readonly Kernel _kernel = kernel;
        private readonly KernelFunction _generateAdventureIntroFunction = KernelFunctionFactory.CreateFromPrompt(File.ReadAllText("Prompts/GenerateAdventureIntroPrompt.txt"));
        private readonly KernelFunction _generateTurnResponseFunction = KernelFunctionFactory.CreateFromPrompt(File.ReadAllText("Prompts/GenerateTurnResponsePrompt.txt"));
        private readonly KernelFunction _generateRecapFunction = KernelFunctionFactory.CreateFromPrompt(File.ReadAllText("Prompts/GenerateRecapPrompt.txt"));

        public Task<string> GenerateAdventureIntroAsync(int characterId, CancellationToken cancellationToken = default)
        {
            var arguments = new KernelArguments
            {
                { "characterId", characterId }
            };

            return _kernel.InvokeAsync(_generateAdventureIntroFunction, arguments, cancellationToken)
                .ContinueWith(task => task.Result.ToString() ?? string.Empty, cancellationToken);
        }

        public Task<string> GenerateRecapAsync(AdventureSession session, CancellationToken cancellationToken = default)
        {
            var arguments = new KernelArguments
            {
                { "sessionSummary", string.Join("\n", session.Events.Select(e => $"Player: {e.PlayerInput}\nDM: {e.AIResponse}")) }
            };
            return _kernel.InvokeAsync(_generateRecapFunction, arguments, cancellationToken)
                .ContinueWith(task => task.Result.ToString() ?? string.Empty, cancellationToken);
        }

        public Task<string> GenerateTurnResponseAsync(AdventureSession session, string playerInput, CancellationToken cancellationToken = default)
        {
            var arguments = new KernelArguments
            {
                { "sessionSummary", string.Join("\n", session.Events.Select(e => $"Player: {e.PlayerInput}\nDM: {e.AIResponse}")) },
                { "playerInput", playerInput }
            };
            return _kernel.InvokeAsync(_generateTurnResponseFunction, arguments, cancellationToken)
                .ContinueWith(task => task.Result.ToString() ?? string.Empty, cancellationToken);
        }
    }
}
