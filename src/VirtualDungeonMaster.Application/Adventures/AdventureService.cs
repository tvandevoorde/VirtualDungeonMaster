using VirtualDungeonMaster.Application.Exceptions;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Narratives;
using VirtualDungeonMaster.Domain.AI;
using VirtualDungeonMaster.Infrastructure.Entities.Adventures;
using VirtualDungeonMaster.Infrastructure.Entities.Characters;
using VirtualDungeonMaster.Infrastructure.Persistance;

namespace VirtualDungeonMaster.Application.Adventures
{
    internal class AdventureService : IAdventureService
    {
        private readonly IAiService _aiService;
        private readonly IAppDbContext _appDbContext;

        public AdventureService(IAiService aiService, IAppDbContext appDbContext)
        {
            _aiService = aiService;
            _appDbContext = appDbContext;
        }

        public async Task<AdventureSession> StartNewSessionAsync(int characterId, string? title = null, CancellationToken cancellationToken = default)
        {
            var character = await _appDbContext.GetCharacterById(characterId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(CharacterEntity), characterId);
            
            var intro = await _aiService.GenerateAdventureIntroAsync(characterId, cancellationToken);

            var session = new AdventureSession(characterId, title ?? "New Adventure");
            session.AddNarrativeEvent("Adventure started", intro);

            await _appDbContext.SaveAsync(session, cancellationToken);

            return session;
        }

        public async Task<AdventureSession?> GetSessionAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            var session = await _appDbContext.GetAdventureSessionById(sessionId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(AdventureSessionEntity), sessionId);

            return session;
        }

        public async Task<NarrativeEvent> SubmitTurnAsync(int sessionId, string playerInput, CancellationToken cancellationToken = default)
        {
            var session = await _appDbContext.GetAdventureSessionById(sessionId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(AdventureSessionEntity), sessionId);

            if (!session.IsActive())
            {
                throw new InvalidActionException("SubmitTurn", "Session is not active");
            }

            var response = await _aiService.GenerateTurnResponseAsync(session, playerInput, cancellationToken);
            session.AddNarrativeEvent(playerInput, response);

            await _appDbContext.SaveAsync(session, cancellationToken);

            return session.GetLastEvent()!;
        }

        public async Task EndSessionAsync(int sessionId, string reason = "completed", CancellationToken cancellationToken = default)
        {
            var session = await _appDbContext.GetAdventureSessionById(sessionId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(AdventureSessionEntity), sessionId);

            if (!session.IsActive())
            {
                throw new InvalidActionException("EndSession", "Session is not active");
            }

            session.EndSession();

            await _appDbContext.SaveAsync(session, cancellationToken);
        }

        public async Task<IReadOnlyList<AdventureSession>> ListSessionsAsync(int characterId, CancellationToken cancellationToken = default)
        {
            var character = await _appDbContext.GetCharacterById(characterId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(CharacterEntity), characterId);

            var sessions = await _appDbContext.GetAdventureSessionsForCharacter(characterId, cancellationToken);

            return sessions;
        }
    }
}
