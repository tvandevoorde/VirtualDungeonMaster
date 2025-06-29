using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Domain.Characters.Repoositories;
using VirtualDungeonMaster.Domain.Characters.Services;

namespace VirtualDungeonMaster.Application.Characters
{
    public class CharacterService(ICharactersRepository charactersRepository) : ICharacterService
    {
        private readonly ICharactersRepository _charactersRepository = charactersRepository;

        public async Task<IReadOnlyList<Character>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _charactersRepository.ListCharactersAsync(cancellationToken);
        }

        public async Task<Character?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _charactersRepository.GetCharacterById(id, cancellationToken);
        }

        public async Task<Character> CreateAsync(Character character, CancellationToken cancellationToken = default)
        {
            return await _charactersRepository.SaveAsync(character, cancellationToken);
        }

        public async Task<Character?> UpdateAsync(int id, Character character, CancellationToken cancellationToken = default)
        {
            return await _charactersRepository.SaveAsync(character, cancellationToken);
        }
    }
}
