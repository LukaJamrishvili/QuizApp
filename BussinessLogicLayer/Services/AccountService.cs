using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

namespace BussinessLogicLayer.Services
{
    public class AccountService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly int _userId;
        public int UserId { get => _userId; }
        public AccountService(int userId, IQuizRepository quizRepository)
        {
            _userId = userId;
            _quizRepository = quizRepository;
        }
        public IEnumerable<Quiz> GetQuizzesWithUserId(int userId)
        {
            return _quizRepository.GetAllQuizOfUser(userId);
        }
        public IEnumerable<Quiz> GetOwnQuizzes()
        {
            return _quizRepository.GetAllQuizOfUser(_userId);
        }
        public Quiz CreateQuiz(Quiz quiz)
        {
            int quizId;
            if (_quizRepository.GetAll().Count() == 0)
            {
                quizId = 1;
            }
            else
            {
                quizId = _quizRepository.GetAll().Select(q => q.Id).Max() + 1;
            }
            return _quizRepository.
                Create(new Quiz(quizId, quiz.OwnerId, quiz.QuizTitle, quiz.Questions));
        }

        public bool UpdateQuiz(Quiz quiz)
        {
            int id = -1;
            id = _quizRepository.GetWithId(quiz.Id).Id;
            if (id > 0)
            {
                _quizRepository.Update(quiz);
                return true;
            }
            return false;
        }

        public bool DeleteQuiz(Quiz quiz)
        {
            if (quiz != null)
            {
                _quizRepository.Delete(quiz.Id);
                return true;
            }
            return false;
        }

        public bool CheckRecord(Quiz quiz, QuizRecord quizRecord)
        {
            if (quiz.QuizRecord.Username == "None")
            {
                quiz.QuizRecord = quizRecord;
                _quizRepository.Update(quiz);
                return true;
            }

            else
            {
                if (quizRecord.Score > quiz.QuizRecord.Score)
                {
                    quiz.QuizRecord = quizRecord;
                    _quizRepository.Update(quiz);
                    return true;

                }
                else if (quizRecord.Score == quiz.QuizRecord.Score)
                {
                    if (quizRecord.LowestTime < quiz.QuizRecord.LowestTime)
                    {
                        quiz.QuizRecord = quizRecord;
                        _quizRepository.Update(quiz);
                        return true;
                    }
                }
            }
            return false;
        }

        public Quiz GetQuiz(int quizId)
        {
            return _quizRepository.GetWithId(quizId);
        }

    }
}
