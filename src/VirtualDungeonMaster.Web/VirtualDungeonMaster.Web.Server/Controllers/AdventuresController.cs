using Microsoft.AspNetCore.Mvc;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Narratives;
using VirtualDungeonMaster.Domain.Adventures.Services;

namespace VirtualDungeonMaster.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdventuresController(IAdventureService adventureService) : ControllerBase
    {
        private readonly IAdventureService _adventureService = adventureService;

        // POST: api/adventures/session
        [HttpPost("session")]
        public async Task<ActionResult<AdventureSession>> StartNewSession([FromBody] StartSessionRequest request, CancellationToken cancellationToken)
        {
            AdventureSession session = await _adventureService.StartNewSessionAsync(request.CharacterId, request.Title, cancellationToken);
            return Ok(session);
        }

        // GET: api/adventures/session/{sessionId}
        [HttpGet("session/{sessionId}")]
        public async Task<ActionResult<AdventureSession>> GetSession(int sessionId, CancellationToken cancellationToken)
        {
            AdventureSession? session = await _adventureService.GetSessionAsync(sessionId, cancellationToken);
            return session == null ? (ActionResult<AdventureSession>)NotFound() : (ActionResult<AdventureSession>)Ok(session);
        }

        // POST: api/adventures/session/{sessionId}/turn
        [HttpPost("session/{sessionId}/turn")]
        public async Task<ActionResult<NarrativeEvent>> SubmitTurn(int sessionId, [FromBody] SubmitTurnRequest request, CancellationToken cancellationToken)
        {
            NarrativeEvent result = await _adventureService.SubmitTurnAsync(sessionId, request.PlayerInput, cancellationToken);
            return Ok(result);
        }

        // POST: api/adventures/session/{sessionId}/end
        [HttpPost("session/{sessionId}/end")]
        public async Task<IActionResult> EndSession(int sessionId, CancellationToken cancellationToken)
        {
            await _adventureService.EndSessionAsync(sessionId, cancellationToken);
            return NoContent();
        }

        // GET: api/adventures/character/{characterId}/sessions
        [HttpGet("character/{characterId}/sessions")]
        public async Task<ActionResult<IReadOnlyList<AdventureSession>>> ListSessions(int characterId, CancellationToken cancellationToken)
        {
            IReadOnlyList<AdventureSession> sessions = await _adventureService.ListSessionsAsync(characterId, cancellationToken);
            return Ok(sessions);
        }

        // Request DTOs
        public class StartSessionRequest
        {
            public int CharacterId { get; set; }
            public string? Title { get; set; }
        }

        public class SubmitTurnRequest
        {
            public string PlayerInput { get; set; } = string.Empty;
        }
    }
}
