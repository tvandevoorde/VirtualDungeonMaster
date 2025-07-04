using NSubstitute;
using VirtualDungeonMaster.Application.Adventures;
using VirtualDungeonMaster.Application.Exceptions;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.Adventures.Repositories;
using VirtualDungeonMaster.Domain.AI;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Domain.Characters.Repoositories;

namespace VirtualDungeonMaster.Tests.AdventureTests
{
    public class AdventureServiceTests
    {
        [Fact]
        public async Task StartNewSessionAsync_ShouldCreateSessionAndSave()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            var character = new Character { Id = 1, Name = "Hero" };
            charactersRepo.GetCharacterById(1, Arg.Any<CancellationToken>()).Returns(character);
            aiService.GenerateAdventureIntroAsync(character, "Epic Quest", Arg.Any<CancellationToken>()).Returns("Welcome!");
            AdventureSession? savedSession = new AdventureSession(1, "Test");
            adventuresRepo.SaveAsync(Arg.Do<AdventureSession>(s => savedSession = s), Arg.Any<CancellationToken>()).Returns(callInfo => savedSession);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            AdventureSession session = await service.StartNewSessionAsync(character.Id, "Epic Quest");

            Assert.NotNull(session);
            Assert.Equal(character.Id, session.CharacterId);
            Assert.Equal("Epic Quest", session.Title);
            Assert.Single(session.Events);
            Assert.Equal("Welcome!", session.Events[0].AIResponse);
            Assert.Equal(savedSession, session);
        }

        [Fact]
        public async Task StartNewSessionAsync_ShouldThrowIfCharacterNotFound()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            charactersRepo.GetCharacterById(1, Arg.Any<CancellationToken>()).Returns((Character?)null);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.StartNewSessionAsync(1));
        }

        [Fact]
        public async Task GetSessionAsync_ShouldReturnSession()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            var session = new AdventureSession(1, "Test");
            adventuresRepo.GetAdventureSessionById(2, Arg.Any<CancellationToken>()).Returns(session);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            AdventureSession? result = await service.GetSessionAsync(2);

            Assert.Equal(session, result);
        }

        [Fact]
        public async Task GetSessionAsync_ShouldThrowIfNotFound()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            adventuresRepo.GetAdventureSessionById(2, Arg.Any<CancellationToken>()).Returns((AdventureSession?)null);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetSessionAsync(2));
        }

        [Fact]
        public async Task SubmitTurnAsync_ShouldAddEventAndSave()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            var session = new AdventureSession(1, "Test");
            adventuresRepo.GetAdventureSessionById(3, Arg.Any<CancellationToken>()).Returns(session);
            aiService.GenerateTurnResponseAsync(session, "go north", Arg.Any<CancellationToken>()).Returns("You go north.");
            adventuresRepo.SaveAsync(session, Arg.Any<CancellationToken>()).Returns(callInfo => Task.FromResult(session));
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            Domain.Adventures.Narratives.NarrativeEvent result = await service.SubmitTurnAsync(3, "go north");

            Assert.NotNull(result);
            Assert.Equal("go north", result.PlayerInput);
            Assert.Equal("You go north.", result.AIResponse);
            await adventuresRepo.Received().SaveAsync(session, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task SubmitTurnAsync_ShouldThrowIfSessionNotFound()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            adventuresRepo.GetAdventureSessionById(3, Arg.Any<CancellationToken>()).Returns((AdventureSession?)null);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.SubmitTurnAsync(3, "go north"));
        }

        [Fact]
        public async Task SubmitTurnAsync_ShouldThrowIfSessionNotActive()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            var session = new AdventureSession(1, "Test");
            session.EndSession();
            adventuresRepo.GetAdventureSessionById(3, Arg.Any<CancellationToken>()).Returns(session);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await Assert.ThrowsAsync<InvalidActionException>(() => service.SubmitTurnAsync(3, "go north"));
        }

        [Fact]
        public async Task EndSessionAsync_ShouldEndAndSave()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            var session = new AdventureSession(1, "Test");
            adventuresRepo.GetAdventureSessionById(4, Arg.Any<CancellationToken>()).Returns(session);
            adventuresRepo.SaveAsync(session, Arg.Any<CancellationToken>()).Returns(callInfo => Task.FromResult(session));
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await service.EndSessionAsync(4);

            Assert.Equal(AdventureStatus.Completed, session.Status);
            await adventuresRepo.Received().SaveAsync(session, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task EndSessionAsync_ShouldThrowIfSessionNotFound()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            adventuresRepo.GetAdventureSessionById(4, Arg.Any<CancellationToken>()).Returns((AdventureSession?)null);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.EndSessionAsync(4));
        }

        [Fact]
        public async Task EndSessionAsync_ShouldThrowIfSessionNotActive()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            var session = new AdventureSession(1, "Test");
            session.EndSession();
            adventuresRepo.GetAdventureSessionById(4, Arg.Any<CancellationToken>()).Returns(session);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await Assert.ThrowsAsync<InvalidActionException>(() => service.EndSessionAsync(4));
        }

        [Fact]
        public async Task ListSessionsAsync_ShouldReturnSessions()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            var character = new Character { Id = 5, Name = "Hero" };
            var sessions = new List<AdventureSession> { new(5, "A") };
            charactersRepo.GetCharacterById(5, Arg.Any<CancellationToken>()).Returns(character);
            adventuresRepo.GetAdventureSessionsForCharacter(5, Arg.Any<CancellationToken>()).Returns(sessions);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            IReadOnlyList<AdventureSession> result = await service.ListSessionsAsync(5);

            Assert.Single(result);
            Assert.Equal(5, result[0].CharacterId);
        }

        [Fact]
        public async Task ListSessionsAsync_ShouldThrowIfCharacterNotFound()
        {
            IAiService aiService = Substitute.For<IAiService>();
            IAdventuresRepository adventuresRepo = Substitute.For<IAdventuresRepository>();
            ICharactersRepository charactersRepo = Substitute.For<ICharactersRepository>();
            charactersRepo.GetCharacterById(5, Arg.Any<CancellationToken>()).Returns((Character?)null);
            var service = new AdventureService(aiService, adventuresRepo, charactersRepo);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.ListSessionsAsync(5));
        }
    }
}
