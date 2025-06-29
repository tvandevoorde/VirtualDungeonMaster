using System.ComponentModel;

namespace VirtualDungeonMaster.Infrastructure.Entities.Narratives
{
    public class NarrativeEventEntity : EntityBase
    {
        [Description("The turn sequence number")]
        public int TurnNumber { get; set; }

        [Description("The input of the player")]
        public string PlayerInput { get; set; } = string.Empty;

        [Description("The response of the AI on the player input")]
        public string AIResponse { get; set; } = string.Empty;

        [Description("The timestamp of the event")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
