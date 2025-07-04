using System;

namespace VirtualDungeonMaster.Web.Server.Dtos
{
    public class NarrativeEventDto
    {
        public int Id { get; set; }
        public int TurnNumber { get; set; }
        public string PlayerInput { get; set; } = string.Empty;
        public string AIResponse { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
