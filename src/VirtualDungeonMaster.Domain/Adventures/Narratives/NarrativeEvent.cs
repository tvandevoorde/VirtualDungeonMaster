namespace VirtualDungeonMaster.Domain.Adventures.Narratives
{
    public class NarrativeEvent(int turnNumber, string input, string response)
    {
        /// <summary>
        /// The unique identifier for NarrativeEvent
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The turn sequence number
        /// </summary>
        public int TurnNumber { get; private set; } = turnNumber;

        /// <summary>
        /// The input of the player
        /// </summary>
        public string PlayerInput { get; private set; } = input;

        /// <summary>
        /// The response of the AI on the player input
        /// </summary>
        public string AIResponse { get; private set; } = response;

        /// <summary>
        /// The timestamp of the event
        /// </summary>
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
    }
}
