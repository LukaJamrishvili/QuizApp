using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IQuizRepository : IRepository<Quiz>
    {
        IEnumerable<Quiz> GetAllQuizOfUser(int userId);
    }
}
