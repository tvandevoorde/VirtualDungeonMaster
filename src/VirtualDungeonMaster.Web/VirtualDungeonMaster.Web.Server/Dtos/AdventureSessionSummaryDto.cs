using System;

namespace VirtualDungeonMaster.Web.Server.Dtos
{
    public class AdventureSessionSummaryDto
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int CurrentTurnNumber { get; set; }
    }
}
