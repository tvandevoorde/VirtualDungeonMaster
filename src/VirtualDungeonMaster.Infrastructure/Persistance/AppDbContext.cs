using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Infrastructure.Entities.Adventures;
using VirtualDungeonMaster.Infrastructure.Entities.Characters;
using VirtualDungeonMaster.Infrastructure.Entities.Narratives;
using VirtualDungeonMaster.Infrastructure.Persistance.Configurations;

namespace VirtualDungeonMaster.Infrastructure.Persistance
{
    internal class AppDbContext : DbContext, IAppDbContext
    {
        private readonly IMapper _mapper;

        public AppDbContext(DbContextOptions<AppDbContext> options, IMapper mapper) : base(options)
        {
            _mapper = mapper;
        }

        public DbSet<AdventureSessionEntity> AdventureSessions { get; set; }
        public DbSet<NarrativeEventEntity> NarrativeEvents { get; set; }
        public DbSet<CharacterEntity> Characters { get; set; }

        public async Task<AdventureSession?> GetAdventureSessionById(int id, CancellationToken cancellationToken = default)
        {
            var entity = await AdventureSessions
                .Include(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity == null ? null : _mapper.Map<AdventureSession>(entity);
        }

        public async Task<IReadOnlyList<AdventureSession>> GetAdventureSessionsForCharacter(int characterId, CancellationToken cancellationToken = default)
        {
            var entities = await AdventureSessions
                .Include(x => x.Events)
                .Where(x => x.CharacterId == characterId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<AdventureSession>>(entities);
        }

        public async Task<Character?> GetCharacterById(int id, CancellationToken cancellationToken = default)
        {
            var entity = await Characters.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return entity == null ? null : _mapper.Map<Character>(entity);
        }

        public async Task SaveAsync(AdventureSession session, CancellationToken cancellationToken = default)
        {
            var entity = await AdventureSessions
                .Include(x => x.Events)
                .FirstOrDefaultAsync(x => x.Id == session.Id, cancellationToken);
            if (entity == null)
            {
                entity = _mapper.Map<AdventureSessionEntity>(session);
                await AdventureSessions.AddAsync(entity, cancellationToken);
            }
            else
            {
                _mapper.Map(session, entity);
                AdventureSessions.Update(entity);
            }
            await SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdventureSessionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NarrativeEventEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CharacterEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
