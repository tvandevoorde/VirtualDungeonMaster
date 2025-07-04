using VirtualDungeonMaster.Domain.Adventures.Narratives;

namespace VirtualDungeonMaster.Domain.Adventures.Services
{
    public interface IAdventureService
    {
        /// <summary>
        /// Starts a new adventure session for the specified character.
        /// </summary>
        /// <param name="characterId">The unique identifier of the character for whom the session is being started. Must be a valid, existing
        /// character ID.</param>
        /// <param name="title">An optional title for the session. If <see langword="null"/>, a default title will be assigned.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see
        /// cref="AdventureSession"/>.</returns>
        Task<AdventureSession> StartNewSessionAsync(int characterId, string? title = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves an adventure session by its unique identifier.
        /// </summary>
        /// <remarks>Use this method to retrieve details about a specific adventure session. If the
        /// session does not exist, the method returns <see langword="null"/>.</remarks>
        /// <param name="sessionId">The unique identifier of the session to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see
        /// cref="AdventureSession"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<AdventureSession?> GetSessionAsync(int sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submits the player's turn to the game session and processes the resulting narrative event.
        /// </summary>
        /// <param name="sessionId">The unique identifier of the game session to which the turn belongs.</param>
        /// <param name="playerInput">The player's input for the current turn. Cannot be null or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="NarrativeEvent"/>
        /// generated as a result of processing the player's input.</returns>
        Task<NarrativeEvent> SubmitTurnAsync(int sessionId, string playerInput, CancellationToken cancellationToken = default);

        /// <summary>
        /// Ends the session with the specified session ID asynchronously.
        /// </summary>
        /// <remarks>This method ensures that all resources associated with the session are released.  If
        /// the session is already ended, an exception will be thrown.</remarks>
        /// <param name="sessionId">The unique identifier of the session to end. Must be a valid, active session ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Passing a canceled token will result in the task being
        /// canceled.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task EndSessionAsync(int sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously retrieves a list of adventure sessions associated with the specified character.
        /// </summary>
        /// <param name="characterId">The unique identifier of the character whose adventure sessions are to be retrieved. Must be a positive
        /// integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of <see
        /// cref="AdventureSession"/> objects associated with the specified character. If no sessions are found, the
        /// list will be empty.</returns>
        Task<IReadOnlyList<AdventureSession>> ListSessionsAsync(int characterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a summary of the current state of the specified adventure session.
        /// </summary>
        /// <param name="session">The <see cref="AdventureSession"/> instance representing the adventure session to summarize. Cannot be <see
        /// langword="null"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a string
        /// summarizing the adventure session.</returns>
        Task<string> SummarizeAsync(AdventureSession session, CancellationToken cancellationToken);
    }
}
