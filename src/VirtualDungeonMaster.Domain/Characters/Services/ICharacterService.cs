namespace VirtualDungeonMaster.Domain.Characters.Services
{
    public interface ICharacterService
    {
        Task<IReadOnlyList<Character>> ListAsync(CancellationToken cancellationToken = default);
        Task<Character?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Character> CreateAsync(Character character, CancellationToken cancellationToken = default);
        Task<Character?> UpdateAsync(int id, Character character, CancellationToken cancellationToken = default);
    }
}
