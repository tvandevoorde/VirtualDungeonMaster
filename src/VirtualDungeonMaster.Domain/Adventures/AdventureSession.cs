using VirtualDungeonMaster.Domain.Adventures.Narratives;

namespace VirtualDungeonMaster.Domain.Adventures
{
    public class AdventureSession(int characterId, string title)
    {
        public int Id { get; private set; }
        public int CharacterId { get; private set; } = characterId;
        public string Title { get; private set; } = title;
        public AdventureStatus Status { get; private set; } = AdventureStatus.Active;
        public DateTime StartedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? EndedAt { get; private set; }

        public List<NarrativeEvent> Events { get; private set; } = [];
        public int CurrentTurnNumber { get; private set; } = 0;

        public string CurrentPrompt { get; private set; } = string.Empty;

        public void AddNarrativeEvent(string playerInput, string aiResponse)
        {
            var ev = new NarrativeEvent(CurrentTurnNumber + 1, playerInput, aiResponse);
            Events.Add(ev);
            CurrentTurnNumber++;
            CurrentPrompt = aiResponse;
        }

        public void EndSession()
        {
            Status = AdventureStatus.Completed;
            EndedAt = DateTime.UtcNow;
        }

        public bool IsActive() => Status == AdventureStatus.Active;

        public NarrativeEvent? GetLastEvent()
        {
            return Events.OrderByDescending(e => e.TurnNumber).FirstOrDefault();
        }
    }
}
