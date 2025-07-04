namespace VirtualDungeonMaster.Infrastructure.Persistance
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
