using NSubstitute;
using VirtualDungeonMaster.Application.Characters;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Domain.Characters.Repoositories;

namespace VirtualDungeonMaster.Tests.CharacterTests
{
    public class CharacterServiceTests
    {
        [Fact]
        public async Task ListAsync_ReturnsCharacters()
        {
            ICharactersRepository repo = Substitute.For<ICharactersRepository>();
            var expected = new List<Character> { new() { Id = 1, Name = "Test" } };
            repo.ListCharactersAsync(Arg.Any<CancellationToken>()).Returns(expected);
            var service = new CharacterService(repo);

            IReadOnlyList<Character> result = await service.ListAsync();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCharacter()
        {
            ICharactersRepository repo = Substitute.For<ICharactersRepository>();
            var character = new Character { Id = 2, Name = "Hero" };
            repo.GetCharacterById(2, Arg.Any<CancellationToken>()).Returns(character);
            var service = new CharacterService(repo);

            Character? result = await service.GetByIdAsync(2);

            Assert.Equal(character, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNullIfNotFound()
        {
            ICharactersRepository repo = Substitute.For<ICharactersRepository>();
            repo.GetCharacterById(3, Arg.Any<CancellationToken>()).Returns((Character?)null);
            var service = new CharacterService(repo);

            Character? result = await service.GetByIdAsync(3);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_SavesCharacter()
        {
            ICharactersRepository repo = Substitute.For<ICharactersRepository>();
            var character = new Character { Id = 4, Name = "New" };
            repo.SaveAsync(character, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            var service = new CharacterService(repo);

            Character result = await service.CreateAsync(character);

            await repo.Received().SaveAsync(character, Arg.Any<CancellationToken>());
            Assert.Equal(character, result);
        }

        [Fact]
        public async Task UpdateAsync_SavesCharacter()
        {
            ICharactersRepository repo = Substitute.For<ICharactersRepository>();
            var character = new Character { Id = 5, Name = "Update" };
            repo.SaveAsync(character, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
            var service = new CharacterService(repo);

            Character? result = await service.UpdateAsync(5, character);

            await repo.Received().SaveAsync(character, Arg.Any<CancellationToken>());
            Assert.Equal(character, result);
        }
    }
}
