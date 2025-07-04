namespace VirtualDungeonMaster.Application.Exceptions
{
    public class InvalidActionException(string actionName, string message) : Exception($"Action {actionName} is invalid: {message}")
    {
    }
}
