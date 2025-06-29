using VirtualDungeonMaster.Application.Exceptions;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Narratives;
using VirtualDungeonMaster.Domain.Adventures.Repositories;
using VirtualDungeonMaster.Domain.Adventures.Services;
using VirtualDungeonMaster.Domain.AI;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Domain.Characters.Repoositories;
using VirtualDungeonMaster.Infrastructure.Entities.Adventures;
using VirtualDungeonMaster.Infrastructure.Entities.Characters;

namespace VirtualDungeonMaster.Application.Adventures
{
    internal class AdventureService(IAiService aiService, IAdventuresRepository adventuresRepository, ICharactersRepository charactersRepository) : IAdventureService
    {
        private readonly IAiService _aiService = aiService;
        private readonly IAdventuresRepository _adventuresRepository = adventuresRepository;
        private readonly ICharactersRepository _charactersRepository = charactersRepository;

        public async Task<AdventureSession> StartNewSessionAsync(int characterId, string? title = null, CancellationToken cancellationToken = default)
        {
            Character character = await _charactersRepository.GetCharacterById(characterId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(CharacterEntity), characterId);
            
            var intro = await _aiService.GenerateAdventureIntroAsync(character, title, cancellationToken);

            var session = new AdventureSession(characterId, title ?? "New Adventure");
            session.AddNarrativeEvent("Adventure started", intro);

            session = await _adventuresRepository.SaveAsync(session, cancellationToken);

            return session;
        }

        public async Task<AdventureSession?> GetSessionAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            AdventureSession session = await _adventuresRepository.GetAdventureSessionById(sessionId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(AdventureSessionEntity), sessionId);

            return session;
        }

        public async Task<NarrativeEvent> SubmitTurnAsync(int sessionId, string playerInput, CancellationToken cancellationToken = default)
        {
            AdventureSession session = await _adventuresRepository.GetAdventureSessionById(sessionId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(AdventureSessionEntity), sessionId);

            if (!session.IsActive())
            {
                throw new InvalidActionException("SubmitTurn", "Session is not active");
            }

            var response = await _aiService.GenerateTurnResponseAsync(session, playerInput, cancellationToken);
            session.AddNarrativeEvent(playerInput, response);

            await _adventuresRepository.SaveAsync(session, cancellationToken);

            return session.GetLastEvent()!;
        }

        public async Task EndSessionAsync(int sessionId, string reason = "completed", CancellationToken cancellationToken = default)
        {
            AdventureSession session = await _adventuresRepository.GetAdventureSessionById(sessionId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(AdventureSessionEntity), sessionId);

            if (!session.IsActive())
            {
                throw new InvalidActionException("EndSession", "Session is not active");
            }

            session.EndSession();

            await _adventuresRepository.SaveAsync(session, cancellationToken);
        }

        public async Task<IReadOnlyList<AdventureSession>> ListSessionsAsync(int characterId, CancellationToken cancellationToken = default)
        {
            Character character = await _charactersRepository.GetCharacterById(characterId, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(CharacterEntity), characterId);

            IReadOnlyList<AdventureSession> sessions = await _adventuresRepository.GetAdventureSessionsForCharacter(characterId, cancellationToken);

            return sessions;
        }
    }
}
