namespace VirtualDungeonMaster.Infrastructure.Entities.Narratives
{
    public class NarrativeEventEntity : EntityBase
    {
        public int TurnNumber { get; set; }
        public string PlayerInput { get; set; } = string.Empty;
        public string AIResponse { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
