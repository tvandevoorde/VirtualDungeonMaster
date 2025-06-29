using Microsoft.AspNetCore.Mvc;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Domain.Characters.Services;

namespace VirtualDungeonMaster.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController(ICharacterService characterService) : ControllerBase
    {
        private readonly ICharacterService _characterService = characterService;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Character>>> List(CancellationToken cancellationToken)
        {
            IReadOnlyList<Character> characters = await _characterService.ListAsync(cancellationToken);

            return Ok(characters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> Get(int id, CancellationToken cancellationToken)
        {
            Character? character = await _characterService.GetByIdAsync(id, cancellationToken);

            return character == null ? NotFound() : Ok(character);
        }

        [HttpPost]
        public async Task<ActionResult<Character>> Create([FromBody] Character character, CancellationToken cancellationToken)
        {
            Character created = await _characterService.CreateAsync(character, cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Character>> Update(int id, [FromBody] Character character, CancellationToken cancellationToken)
        {
            Character? updated = await _characterService.UpdateAsync(id, character, cancellationToken);

            return updated == null ? NotFound() : Ok(updated);
        }
    }
}
