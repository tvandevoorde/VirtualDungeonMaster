using VirtualDungeonMaster.Domain.Adventures.Narratives;

namespace VirtualDungeonMaster.Domain.Adventures.Services
{
    public interface IAdventureService
    {
        /// <summary>
        /// Starts a new solo adventure session for the given character.
        /// </summary>
        Task<AdventureSession> StartNewSessionAsync(int characterId, string? title = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an existing adventure session by ID.
        /// </summary>
        Task<AdventureSession?> GetSessionAsync(int sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Handles the next turn in the adventure by sending player input to the AI.
        /// </summary>
        Task<NarrativeEvent> SubmitTurnAsync(int sessionId, string playerInput, CancellationToken cancellationToken = default);

        /// <summary>
        /// Ends a session explicitly.
        /// </summary>
        Task EndSessionAsync(int sessionId, string reason = "completed", CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all active sessions for a given character.
        /// </summary>
        Task<IReadOnlyList<AdventureSession>> ListSessionsAsync(int characterId, CancellationToken cancellationToken = default);
    }
}
