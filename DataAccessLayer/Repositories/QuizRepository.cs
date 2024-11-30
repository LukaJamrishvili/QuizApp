using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.DataLoadingHelper;

namespace DataAccessLayer.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly string _filePath;
        private List<Quiz> _quizzes;

        public QuizRepository(string filePath)
        {
            _filePath = filePath;
            _quizzes = DataLoader<Quiz>.GetData(filePath).ToList();
        }


        public Quiz Create(Quiz newEntity)
        {
            _quizzes.Add(newEntity);
            DataSaver<Quiz>.SaveData(_filePath, _quizzes);
            return newEntity;
        }

        public void Delete(int id)
        {
            Quiz target = _quizzes.FirstOrDefault(q => q.Id == id);
            if (target != null)
            {
                _quizzes.Remove(target);
                DataSaver<Quiz>.SaveData(_filePath, _quizzes);
            }
        }

        public IEnumerable<Quiz> GetAll() => _quizzes;

        public IEnumerable<Quiz> GetAllQuizOfUser(int userId) => _quizzes.Where(q => q.OwnerId == userId);

        public Quiz GetWithId(int id) => _quizzes.FirstOrDefault(q => q.Id == id);

        public Quiz Update(Quiz entity)
        {
            _quizzes[_quizzes.FindIndex(u => u.Id == entity.Id)] = entity;
            DataSaver<Quiz>.SaveData(_filePath, _quizzes);
            return entity;
        }
    }
}
