using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Domain.Characters.Repoositories;
using VirtualDungeonMaster.Infrastructure.Entities.Characters;

namespace VirtualDungeonMaster.Infrastructure.Persistance.Repositories
{
    public class CharactersRepository : ICharactersRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CharactersRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Character?> GetCharacterById(int id, CancellationToken cancellationToken = default)
        {
            CharacterEntity? entity = await _dbContext.Characters
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity == null ? null : _mapper.Map<Character>(entity);
        }

        public async Task<IReadOnlyList<Character>> ListCharactersAsync(CancellationToken cancellationToken = default)
        {
            List<CharacterEntity> characters = await _dbContext.Characters
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<Character>>(characters);
        }

        public async Task<Character> SaveAsync(Character character, CancellationToken cancellationToken = default)
        {
            CharacterEntity? entity = await _dbContext.Characters
                .FirstOrDefaultAsync(x => x.Id == character.Id, cancellationToken);
            if (entity == null)
            {
                entity = _mapper.Map<CharacterEntity>(character);
                await _dbContext.Characters.AddAsync(entity, cancellationToken);
            }
            else
            {
                _mapper.Map(character, entity);
                _dbContext.Characters.Update(entity);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return _mapper.Map<Character>(entity);
        }
    }
}
