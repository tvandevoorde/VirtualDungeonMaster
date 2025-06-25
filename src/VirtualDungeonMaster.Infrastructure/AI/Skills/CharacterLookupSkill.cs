using System.ComponentModel;
using System.Text;
using Microsoft.SemanticKernel;
using VirtualDungeonMaster.Infrastructure.Persistance;

namespace VirtualDungeonMaster.Infrastructure.AI.Skills
{
    public class CharacterLookupSkill(IAppDbContext appDbContext)
    {
        [KernelFunction, Description("Get character info by Id")]
        public async Task<string> GetCharacterByIdAsync([Description("The Id of the character")] int characterId)
        {
            var character = await appDbContext.GetCharacterById(characterId);
            if (character == null)
            {
                return $"Character with Id {characterId} not found.";
            }
            var characterInfo = new StringBuilder();
            characterInfo.AppendLine($"Character ID: {character.Id}");
            characterInfo.AppendLine($"Name: {character.Name}");
            characterInfo.AppendLine($"Class: {character.Class}");
            characterInfo.AppendLine($"Race: {character.Race}");
            characterInfo.AppendLine($"Background: {character.Background}");
            return characterInfo.ToString();
        }
    }
}
