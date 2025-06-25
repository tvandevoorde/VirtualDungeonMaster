using NSubstitute;
using VirtualDungeonMaster.Application.Adventures;
using VirtualDungeonMaster.Application.Exceptions;
using VirtualDungeonMaster.Domain.Adventures;
using VirtualDungeonMaster.Domain.AI;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Infrastructure.Persistance;

namespace VirtualDungeonMaster.Tests.AdventureTests
{
    public class AdventureServiceTests
    {
        [Fact]
        public async Task StartNewSessionAsync_ShouldCreateSessionAndSave()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            var character = new Character { Id = 1, Name = "Hero" };
            dbContext.GetCharacterById(1, Arg.Any<CancellationToken>()).Returns(character);
            aiService.GenerateAdventureIntroAsync(1, Arg.Any<CancellationToken>()).Returns("Welcome!");
            AdventureSession? savedSession = null;
            dbContext.SaveAsync(Arg.Do<AdventureSession>(s => savedSession = s), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            var service = new AdventureService(aiService, dbContext);

            var session = await service.StartNewSessionAsync(1, "Epic Quest");

            Assert.NotNull(session);
            Assert.Equal(1, session.CharacterId);
            Assert.Equal("Epic Quest", session.Title);
            Assert.Single(session.Events);
            Assert.Equal("Welcome!", session.Events[0].AIResponse);
            Assert.Equal(savedSession, session);
        }

        [Fact]
        public async Task StartNewSessionAsync_ShouldThrowIfCharacterNotFound()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            dbContext.GetCharacterById(1, Arg.Any<CancellationToken>()).Returns((Character?)null);
            var service = new AdventureService(aiService, dbContext);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.StartNewSessionAsync(1));
        }

        [Fact]
        public async Task GetSessionAsync_ShouldReturnSession()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            var session = new AdventureSession(1, "Test");
            dbContext.GetAdventureSessionById(2, Arg.Any<CancellationToken>()).Returns(session);
            var service = new AdventureService(aiService, dbContext);

            var result = await service.GetSessionAsync(2);

            Assert.Equal(session, result);
        }

        [Fact]
        public async Task GetSessionAsync_ShouldThrowIfNotFound()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            dbContext.GetAdventureSessionById(2, Arg.Any<CancellationToken>()).Returns((AdventureSession?)null);
            var service = new AdventureService(aiService, dbContext);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetSessionAsync(2));
        }

        [Fact]
        public async Task SubmitTurnAsync_ShouldAddEventAndSave()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            var session = new AdventureSession(1, "Test");
            dbContext.GetAdventureSessionById(3, Arg.Any<CancellationToken>()).Returns(session);
            aiService.GenerateTurnResponseAsync(session, "go north", Arg.Any<CancellationToken>()).Returns("You go north.");
            var service = new AdventureService(aiService, dbContext);

            var result = await service.SubmitTurnAsync(3, "go north");

            Assert.NotNull(result);
            Assert.Equal("go north", result.PlayerInput);
            Assert.Equal("You go north.", result.AIResponse);
            await dbContext.Received().SaveAsync(session, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task SubmitTurnAsync_ShouldThrowIfSessionNotFound()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            dbContext.GetAdventureSessionById(3, Arg.Any<CancellationToken>()).Returns((AdventureSession?)null);
            var service = new AdventureService(aiService, dbContext);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.SubmitTurnAsync(3, "go north"));
        }

        [Fact]
        public async Task SubmitTurnAsync_ShouldThrowIfSessionNotActive()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            var session = new AdventureSession(1, "Test");
            session.EndSession();
            dbContext.GetAdventureSessionById(3, Arg.Any<CancellationToken>()).Returns(session);
            var service = new AdventureService(aiService, dbContext);

            await Assert.ThrowsAsync<InvalidActionException>(() => service.SubmitTurnAsync(3, "go north"));
        }

        [Fact]
        public async Task EndSessionAsync_ShouldEndAndSave()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            var session = new AdventureSession(1, "Test");
            dbContext.GetAdventureSessionById(4, Arg.Any<CancellationToken>()).Returns(session);
            var service = new AdventureService(aiService, dbContext);

            await service.EndSessionAsync(4, "completed");

            Assert.Equal(Domain.Adventures.AdventureStatus.Completed, session.Status);
            await dbContext.Received().SaveAsync(session, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task EndSessionAsync_ShouldThrowIfSessionNotFound()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            dbContext.GetAdventureSessionById(4, Arg.Any<CancellationToken>()).Returns((AdventureSession?)null);
            var service = new AdventureService(aiService, dbContext);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.EndSessionAsync(4));
        }

        [Fact]
        public async Task EndSessionAsync_ShouldThrowIfSessionNotActive()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            var session = new AdventureSession(1, "Test");
            session.EndSession();
            dbContext.GetAdventureSessionById(4, Arg.Any<CancellationToken>()).Returns(session);
            var service = new AdventureService(aiService, dbContext);

            await Assert.ThrowsAsync<InvalidActionException>(() => service.EndSessionAsync(4));
        }

        [Fact]
        public async Task ListSessionsAsync_ShouldReturnSessions()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            var character = new Character { Id = 5, Name = "Hero" };
            var sessions = new List<AdventureSession> { new AdventureSession(5, "A") };
            dbContext.GetCharacterById(5, Arg.Any<CancellationToken>()).Returns(character);
            dbContext.GetAdventureSessionsForCharacter(5, Arg.Any<CancellationToken>()).Returns(sessions);
            var service = new AdventureService(aiService, dbContext);

            var result = await service.ListSessionsAsync(5);

            Assert.Single(result);
            Assert.Equal(5, result[0].CharacterId);
        }

        [Fact]
        public async Task ListSessionsAsync_ShouldThrowIfCharacterNotFound()
        {
            var aiService = Substitute.For<IAiService>();
            var dbContext = Substitute.For<IAppDbContext>();
            dbContext.GetCharacterById(5, Arg.Any<CancellationToken>()).Returns((Character?)null);
            var service = new AdventureService(aiService, dbContext);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => service.ListSessionsAsync(5));
        }
    }
}
