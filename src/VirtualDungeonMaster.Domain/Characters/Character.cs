namespace VirtualDungeonMaster.Domain.Characters
{
    public class Character
    {
        /// <summary>
        /// The unique id of the character
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the character
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The class of the character
        /// </summary>
        public string Class { get; set; } = string.Empty;

        /// <summary>
        /// The race of the character
        /// </summary>
        public string Race { get; set; } = string.Empty;

        /// <summary>
        /// The background story of the character
        /// </summary>
        public string Background { get; set; } = string.Empty;
    }
}
