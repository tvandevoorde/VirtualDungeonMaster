namespace VirtualDungeonMaster.Domain.Characters.Repoositories
{
    public interface ICharactersRepository
    {
        /// <summary>
        /// Retrieves a character by its unique identifier.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch the character data.  Ensure
        /// the <paramref name="Id"/> parameter is valid and within the expected range.</remarks>
        /// <param name="Id">The unique identifier of the character to retrieve. Must be a positive integer.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>A <see cref="Character"/> object representing the character with the specified identifier,  or <see
        /// langword="null"/> if no character with the given identifier exists.</returns>
        Task<Character?> GetCharacterById(int Id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a list of all characters in the system.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch all characters. It returns a read-only list of <see cref="Character"/> objects.</remarks>
        Task<IReadOnlyList<Character>> ListCharactersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saved the specified character asynchronously.
        /// </summary>
        /// <param name="character">The <see cref="Character"/> instance to be saved. Cannot be <see langword="null"/>.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests. Defaults to <see langword="default"/> if not provided.</param>
        /// <returns>The saved character</returns>
        Task<Character> SaveAsync(Character character, CancellationToken cancellationToken = default);
    }
}
