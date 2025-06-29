using System.ComponentModel;

namespace VirtualDungeonMaster.Infrastructure.Entities.Characters
{
    public class CharacterEntity : EntityBase
    {
        [Description("The name of the character")]
        public string Name { get; set; } = string.Empty;

        [Description("The class of the character")]
        public string Class { get; set; } = string.Empty;

        [Description("The race of the character")]
        public string Race { get; set; } = string.Empty;

        [Description("The background story of the character")]
        public string Background { get; set; } = string.Empty;
    }
}
