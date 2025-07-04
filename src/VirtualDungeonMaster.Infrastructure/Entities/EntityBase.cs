using System.ComponentModel;

namespace VirtualDungeonMaster.Infrastructure.Entities
{
    public abstract class EntityBase
    {
        [Description("The unique identifier for this entitiy")]
        public int Id { get; set; }
    }
}
