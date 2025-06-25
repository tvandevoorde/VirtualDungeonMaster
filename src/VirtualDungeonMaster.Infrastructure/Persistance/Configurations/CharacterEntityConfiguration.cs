using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VirtualDungeonMaster.Infrastructure.Entities.Characters;

namespace VirtualDungeonMaster.Infrastructure.Persistance.Configurations
{
    public class CharacterEntityConfiguration : IEntityTypeConfiguration<CharacterEntity>
    {
        public void Configure(EntityTypeBuilder<CharacterEntity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Class).HasMaxLength(50);
            builder.Property(e => e.Race).HasMaxLength(50);
            builder.Property(e => e.Background).HasMaxLength(200);
        }
    }
}
