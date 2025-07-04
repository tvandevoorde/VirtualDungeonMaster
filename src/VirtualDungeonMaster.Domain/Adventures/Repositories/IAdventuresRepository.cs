namespace VirtualDungeonMaster.Domain.Adventures.Repositories
{
    public interface IAdventuresRepository
    {
        /// <summary>
        /// Retrieves an adventure session by its unique identifier.
        /// </summary>
        /// <remarks>Use this method to retrieve details of a specific adventure session. If no session
        /// exists with the given identifier, the method returns <see langword="null"/>.</remarks>
        /// <param name="id">The unique identifier of the adventure session to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see
        /// cref="AdventureSession"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<AdventureSession?> GetAdventureSessionById(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of adventure sessions associated with the specified character.
        /// </summary>
        /// <param name="characterId">The unique identifier of the character whose adventure sessions are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of  <see
        /// cref="AdventureSession"/> objects associated with the specified character. If no sessions are found, the
        /// list will be empty.</returns>
        Task<IReadOnlyList<AdventureSession>> GetAdventureSessionsForCharacter(int characterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves the specified adventure session asynchronously.
        /// </summary>
        /// <remarks>This method performs an asynchronous save operation for the provided adventure
        /// session. Ensure that the session object is properly initialized before calling this method.</remarks>
        /// <param name="session">The <see cref="AdventureSession"/> instance to be saved. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the save operation. Defaults to <see
        /// cref="CancellationToken.None"/>.</param>
        /// <returns>The saved adventure session</returns>
        Task<AdventureSession> SaveAsync(AdventureSession session, CancellationToken cancellationToken = default);
    }
}
