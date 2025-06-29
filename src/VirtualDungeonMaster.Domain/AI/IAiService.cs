using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Characters;

namespace VirtualDungeonMaster.Domain.AI
{
    public interface IAiService
    {
        /// <summary>
        /// Generates the opening narrative for a new adventure session based on the player's character.
        /// </summary>
        /// <param name="character">The character participating in the adventure</param>
        /// <param name="title">The title of the adventure</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GenerateAdventureIntroAsync(Character character, string? title, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates the AI's narrative response to a player's turn input.
        /// </summary>
        /// <param name="session">The current AdventureSession</param>
        /// <param name="playerInput">The input of the player</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GenerateTurnResponseAsync(AdventureSession session, string playerInput, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a short summary or recap of the current session state.
        /// </summary>
        /// <param name="session">The current AdventureSession</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GenerateRecapAsync(AdventureSession session, CancellationToken cancellationToken = default);
    }
}
