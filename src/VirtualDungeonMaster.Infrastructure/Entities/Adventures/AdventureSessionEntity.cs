using System.ComponentModel;
using VirtualDungeonMaster.Infrastructure.Entities.Narratives;

namespace VirtualDungeonMaster.Infrastructure.Entities.Adventures
{
    public class AdventureSessionEntity : EntityBase
    {
        [Description("The unique identifier of the character that participates in this adventure")]
        public int CharacterId { get; set; }

        [Description("The title of the adventure")]
        public string Title { get; set; } = string.Empty;

        [Description("The status of the adventure")]
        public string Status { get; set; } = "Active";

        [Description("The date and time of when the adventure started")]
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        [Description("The date and time of when the adventure ended")]
        public DateTime? EndedAt { get; set; }

        [Description("The events that occured in the adventure")]
        public List<NarrativeEventEntity> Events { get; set; } = [];

        [Description("The current turn sequence number of the adventure")]
        public int CurrentTurnNumber { get; set; } = 0;

        [Description("The current prompt of the adventure")]
        public string CurrentPrompt { get; set; } = string.Empty;
    }
}
