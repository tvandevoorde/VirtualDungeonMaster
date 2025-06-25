namespace VirtualDungeonMaster.Infrastructure.Entities.Characters
{
    public class CharacterEntity : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
    }
}
