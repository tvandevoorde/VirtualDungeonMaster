using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualDungeonMaster.Infrastructure.Entities.Adventures;

namespace VirtualDungeonMaster.Infrastructure.Persistance.Configurations
{
    public class AdventureSessionEntityConfiguration : IEntityTypeConfiguration<AdventureSessionEntity>
    {
        public void Configure(EntityTypeBuilder<AdventureSessionEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Status).IsRequired().HasMaxLength(50);
            builder.Property(e => e.StartedAt).IsRequired();
            builder.Property(e => e.CurrentPrompt).HasMaxLength(1000);
            builder.HasMany(e => e.Events).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
