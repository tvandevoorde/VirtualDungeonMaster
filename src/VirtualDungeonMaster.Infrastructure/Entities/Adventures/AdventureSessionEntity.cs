using VirtualDungeonMaster.Infrastructure.Entities.Narratives;

namespace VirtualDungeonMaster.Infrastructure.Entities.Adventures
{
    public class AdventureSessionEntity : EntityBase
    {
        public int CharacterId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; set; }
        public List<NarrativeEventEntity> Events { get; set; } = [];
        public int CurrentTurnNumber { get; set; } = 0;
        public string CurrentPrompt { get; set; } = string.Empty;
    }
}
