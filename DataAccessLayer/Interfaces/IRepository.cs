using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetWithId(int id);
        IEnumerable<T> GetAll();
        T Create(T newEntity);
        T Update(T entity);
        void Delete(int id);
    }
}
