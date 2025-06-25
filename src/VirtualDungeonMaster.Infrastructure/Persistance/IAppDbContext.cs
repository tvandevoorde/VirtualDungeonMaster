using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Characters;

namespace VirtualDungeonMaster.Infrastructure.Persistance
{
    public interface IAppDbContext
    {
        Task<AdventureSession?> GetAdventureSessionById(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AdventureSession>> GetAdventureSessionsForCharacter(int characterId, CancellationToken cancellationToken = default);
        Task SaveAsync(AdventureSession session, CancellationToken cancellationToken = default);

        Task<Character?> GetCharacterById(int Id, CancellationToken cancellationToken = default);
    }
}
