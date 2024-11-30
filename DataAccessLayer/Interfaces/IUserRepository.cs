using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        bool Contains(string username);

        User GetWithUsername(string username);
    }
}
