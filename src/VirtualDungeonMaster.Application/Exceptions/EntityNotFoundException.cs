namespace VirtualDungeonMaster.Application.Exceptions
{
    public class EntityNotFoundException(string entityType, object identifier) : Exception($"Entity {entityType} with identifier {identifier} was not found")
    {
    }
}
