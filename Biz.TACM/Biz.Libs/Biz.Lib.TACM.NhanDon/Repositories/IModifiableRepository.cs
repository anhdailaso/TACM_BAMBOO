namespace Biz.Lib.TACM.NhanDon.Repositories
{
    public interface IModifiableRepository<T> where T : class, new()
    {
        int Create(T entity);

        int Update(T entity);

        bool Delete(object id);
    }
}
