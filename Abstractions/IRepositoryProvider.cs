namespace Abstractions
{
    public interface IRepositoryProvider
    {
        IRepository<T> RepositoryFor<T>() where T : class;
    }
}
