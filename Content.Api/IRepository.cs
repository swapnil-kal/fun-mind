namespace Content.Api
{
    public interface IRepository<T>
    {
        Task<T> GetById(string id);

        Task<T> Create(T t);

        Task<T> Update(T t);

        Task<bool> Delete(T t);
    }
}
