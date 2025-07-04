using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualDungeonMaster.Infrastructure.Entities.Narratives;

namespace VirtualDungeonMaster.Infrastructure.Persistance.Configurations
{
    public class NarrativeEventEntityConfiguration : IEntityTypeConfiguration<NarrativeEventEntity>
    {
        public void Configure(EntityTypeBuilder<NarrativeEventEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.TurnNumber).IsRequired();
            builder.Property(e => e.PlayerInput).HasMaxLength(5000);
            builder.Property(e => e.AIResponse).HasMaxLength(5000);
            builder.Property(e => e.Timestamp).IsRequired();
        }
    }
}
