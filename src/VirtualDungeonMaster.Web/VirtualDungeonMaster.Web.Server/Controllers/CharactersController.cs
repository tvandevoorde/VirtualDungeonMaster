using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualDungeonMaster.Domain.Characters;
using VirtualDungeonMaster.Domain.Characters.Services;
using VirtualDungeonMaster.Web.Server.Dtos;

namespace VirtualDungeonMaster.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController(ICharacterService characterService, IMapper mapper) : ControllerBase
    {
        private readonly ICharacterService _characterService = characterService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CharacterDto>>> List(CancellationToken cancellationToken)
        {
            IReadOnlyList<Character> characters = await _characterService.ListAsync(cancellationToken);
            IReadOnlyList<CharacterDto> dtos = _mapper.Map<IReadOnlyList<CharacterDto>>(characters);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDto>> Get(int id, CancellationToken cancellationToken)
        {
            Character? character = await _characterService.GetByIdAsync(id, cancellationToken);
            if (character == null)
            {
                return NotFound();
            }

            CharacterDto dto = _mapper.Map<CharacterDto>(character);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CharacterDto>> Create([FromBody] CharacterDto characterDto, CancellationToken cancellationToken)
        {
            Character character = _mapper.Map<Character>(characterDto);
            Character created = await _characterService.CreateAsync(character, cancellationToken);
            CharacterDto createdDto = _mapper.Map<CharacterDto>(created);
            return CreatedAtAction(nameof(Get), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CharacterDto>> Update(int id, [FromBody] CharacterDto characterDto, CancellationToken cancellationToken)
        {
            Character character = _mapper.Map<Character>(characterDto);
            Character? updated = await _characterService.UpdateAsync(id, character, cancellationToken);
            if (updated == null)
            {
                return NotFound();
            }

            CharacterDto updatedDto = _mapper.Map<CharacterDto>(updated);
            return Ok(updatedDto);
        }
    }
}
