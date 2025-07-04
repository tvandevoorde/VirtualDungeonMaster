using VirtualDungeonMaster.Domain.Adventures.Narratives;

namespace VirtualDungeonMaster.Domain.Adventures
{
    public class AdventureSession(int characterId, string title)
    {
        /// <summary>
        /// The unique identifier for the adventure session
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The unique identifier of the character that participates in this adventure
        /// </summary>
        public int CharacterId { get; private set; } = characterId;

        /// <summary>
        /// The title of the adventure
        /// </summary>
        public string Title { get; private set; } = title;

        /// <summary>
        /// The status of the adventure
        /// </summary>
        public AdventureStatus Status { get; private set; } = AdventureStatus.Active;

        /// <summary>
        /// The date and time of when the adventure started
        /// </summary>
        public DateTime StartedAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time of when the adventure ended
        /// </summary>
        public DateTime? EndedAt { get; private set; }

        /// <summary>
        /// The events that occured in the adventure
        /// </summary>
        public List<NarrativeEvent> Events { get; private set; } = [];

        /// <summary>
        /// The current turn sequence number of the adventure
        /// </summary>
        public int CurrentTurnNumber { get; private set; } = 0;

        /// <summary>
        /// The current prompt of the adventure
        /// </summary>
        public string CurrentPrompt { get; private set; } = string.Empty;

        /// <summary>
        /// Adds a new event to the adventure and increases the turn number
        /// </summary>
        /// <param name="playerInput">player input text</param>
        /// <param name="aiResponse">AI response text</param>
        public void AddNarrativeEvent(string playerInput, string aiResponse)
        {
            var ev = new NarrativeEvent(CurrentTurnNumber + 1, playerInput, aiResponse);
            Events.Add(ev);
            CurrentTurnNumber++;
            CurrentPrompt = aiResponse;
        }

        /// <summary>
        /// Ends the adventure session
        /// </summary>
        public void EndSession()
        {
            Status = AdventureStatus.Completed;
            EndedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Returns whether the session is active or not
        /// </summary>
        /// <returns>Active True or False</returns>
        public bool IsActive()
        {
            return Status == AdventureStatus.Active;
        }

        /// <summary>
        /// Get the last event of the adventure session
        /// </summary>
        /// <returns>The last NarrativeEvent or null if no events exist</returns>
        public NarrativeEvent? GetLastEvent()
        {
            return Events.OrderByDescending(e => e.TurnNumber).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all narrative events from the current collection.
        /// </summary>
        /// <remarks>This method creates a new list containing all events in the collection. Modifications
        /// to the returned list will not affect the original collection.</remarks>
        /// <returns>A list of <see cref="NarrativeEvent"/> objects representing all events in the collection. If the collection
        /// is empty, an empty list is returned.</returns>
        public List<NarrativeEvent> GetAllEvents()
        {
            return [.. Events];
        }
    }
}
