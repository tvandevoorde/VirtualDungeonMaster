namespace VirtualDungeonMaster.Domain.Characters
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
    }
}
