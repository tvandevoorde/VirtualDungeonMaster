namespace VirtualDungeonMaster.Domain.Adventures.Narratives
{
    public class NarrativeEvent(int turnNumber, string input, string response)
    {
        public int Id { get; private set; }
        public int TurnNumber { get; private set; } = turnNumber;
        public string PlayerInput { get; private set; } = input;
        public string AIResponse { get; private set; } = response;
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
    }
}
