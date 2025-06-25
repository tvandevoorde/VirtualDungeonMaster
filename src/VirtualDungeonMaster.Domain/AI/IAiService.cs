using VirtualDungeonMaster.Domain.Adventures;

namespace VirtualDungeonMaster.Domain.AI
{
    public interface IAiService
    {
        /// <summary>
        /// Generates the opening narrative for a new adventure session based on the player's character.
        /// </summary>
        Task<string> GenerateAdventureIntroAsync(int characterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates the AI's narrative response to a player's turn input.
        /// </summary>
        Task<string> GenerateTurnResponseAsync(AdventureSession session, string playerInput, CancellationToken cancellationToken = default);

        /// <summary>
        /// Optionally generates a short summary or recap of the current session state.
        /// </summary>
        Task<string> GenerateRecapAsync(AdventureSession session, CancellationToken cancellationToken = default);
    }
}
