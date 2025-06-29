using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Repositories;
using VirtualDungeonMaster.Infrastructure.Entities.Adventures;

namespace VirtualDungeonMaster.Infrastructure.Persistance.Repositories
{
    public class AdventuresRepository : IAdventuresRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public AdventuresRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<AdventureSession?> GetAdventureSessionById(int id, CancellationToken cancellationToken = default)
        {
            AdventureSessionEntity? entity = await _dbContext.AdventureSessions
                .AsNoTracking()
                .Include(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity == null ? null : _mapper.Map<AdventureSession>(entity);
        }

        public async Task<IReadOnlyList<AdventureSession>> GetAdventureSessionsForCharacter(int characterId, CancellationToken cancellationToken = default)
        {
            List<AdventureSessionEntity> entities = await _dbContext.AdventureSessions
                .AsNoTracking()
                .Include(x => x.Events)
                .Where(x => x.CharacterId == characterId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<AdventureSession>>(entities);
        }

        public async Task<AdventureSession> SaveAsync(AdventureSession session, CancellationToken cancellationToken = default)
        {
            AdventureSessionEntity? entity = await _dbContext.AdventureSessions
                .Include(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == session.Id, cancellationToken);
            if (entity == null)
            {
                entity = _mapper.Map<AdventureSessionEntity>(session);
                await _dbContext.AdventureSessions.AddAsync(entity, cancellationToken);
            }
            else
            {
                _mapper.Map(session, entity);
                _dbContext.AdventureSessions.Update(entity);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return _mapper.Map<AdventureSession>(entity);
        }
    }
}
