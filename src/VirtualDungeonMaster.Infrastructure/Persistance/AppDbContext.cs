using Microsoft.EntityFrameworkCore;
using VirtualDungeonMaster.Infrastructure.Entities.Adventures;
using VirtualDungeonMaster.Infrastructure.Entities.Characters;
using VirtualDungeonMaster.Infrastructure.Entities.Narratives;
using VirtualDungeonMaster.Infrastructure.Persistance.Configurations;

namespace VirtualDungeonMaster.Infrastructure.Persistance
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<AdventureSessionEntity> AdventureSessions { get; private set; }
        public DbSet<NarrativeEventEntity> NarrativeEvents { get; private set; }
        public DbSet<CharacterEntity> Characters { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdventureSessionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new NarrativeEventEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CharacterEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
