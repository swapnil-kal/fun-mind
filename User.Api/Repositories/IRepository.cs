namespace User.Api.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);

        Task<bool> Create(T t);

        Task<bool> Update(T t);

        Task Delete(T t);
    }
}
