using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Narratives;
using VirtualDungeonMaster.Domain.Adventures.Services;
using VirtualDungeonMaster.Web.Server.Dtos;

namespace VirtualDungeonMaster.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class AdventuresController(IAdventureService adventureService, IMapper mapper) : ControllerBase
    {
        private readonly IAdventureService _adventureService = adventureService;
        private readonly IMapper _mapper = mapper;

        // POST: api/adventures/session
        [HttpPost("session")]
        public async Task<ActionResult<AdventureSessionDto>> StartNewSession([FromBody] StartSessionRequestDto request, CancellationToken cancellationToken)
        {
            AdventureSession session = await _adventureService.StartNewSessionAsync(request.CharacterId, request.Title, cancellationToken);
            AdventureSessionDto dto = _mapper.Map<AdventureSessionDto>(session);
            return Ok(dto);
        }

        // GET: api/adventures/session/{sessionId}
        [HttpGet("session/{sessionId}")]
        public async Task<ActionResult<AdventureSessionDto>> GetSession(int sessionId, CancellationToken cancellationToken)
        {
            AdventureSession? session = await _adventureService.GetSessionAsync(sessionId, cancellationToken);
            if (session == null)
            {
                return NotFound();
            }

            AdventureSessionDto dto = _mapper.Map<AdventureSessionDto>(session);

            dto.AdventureSummary = await _adventureService.SummarizeAsync(session, cancellationToken);

            return Ok(dto);
        }

        // POST: api/adventures/session/{sessionId}/turn
        [HttpPost("session/{sessionId}/turn")]
        public async Task<ActionResult<NarrativeEventDto>> SubmitTurn(int sessionId, [FromBody] SubmitTurnRequestDto request, CancellationToken cancellationToken)
        {
            NarrativeEvent result = await _adventureService.SubmitTurnAsync(sessionId, request.PlayerInput, cancellationToken);
            NarrativeEventDto dto = _mapper.Map<NarrativeEventDto>(result);
            return Ok(dto);
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
        public async Task<ActionResult<IReadOnlyList<AdventureSessionSummaryDto>>> ListSessions(int characterId, CancellationToken cancellationToken)
        {
            IReadOnlyList<AdventureSession> sessions = await _adventureService.ListSessionsAsync(characterId, cancellationToken);
            IReadOnlyList<AdventureSessionSummaryDto> dtos = _mapper.Map<IReadOnlyList<AdventureSessionSummaryDto>>(sessions);
            return Ok(dtos);
        }

        // GET: api/adventures/session/{sessionId}/events
        [HttpGet("session/{sessionId}/events")]
        public async Task<ActionResult<IReadOnlyList<NarrativeEventDto>>> GetSessionEvents(
            int sessionId,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 20,
            CancellationToken cancellationToken = default)
        {
            AdventureSession? session = await _adventureService.GetSessionAsync(sessionId, cancellationToken);
            if (session == null)
            {
                return NotFound();
            }
            var events = session.GetAllEvents()
                .OrderByDescending(e => e.TurnNumber)
                .Skip(skip)
                .Take(take)
                .ToList();
            IReadOnlyList<NarrativeEventDto> dtos = _mapper.Map<IReadOnlyList<NarrativeEventDto>>(events);
            return Ok(dtos);
        }
    }
}
