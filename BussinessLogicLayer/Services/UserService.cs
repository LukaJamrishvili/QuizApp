using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using QuizApp;

namespace BussinessLogicLayer.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public readonly LogInValidator _logInValidator;
        public readonly RegistrationValidator _registrationValidator;
        public UserService(IUserRepository userRepository)
        {
            _logInValidator = new LogInValidator(userRepository);
            _registrationValidator = new RegistrationValidator(userRepository);
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAllUsers() => _userRepository.GetAll();
        public void UpdateQuizRecord(User user, QuizRecord quizRecord)
        {
            QuizRecord? currentRecord = null;
            int index = 0;
            foreach (QuizRecord record in user.AttemptedQuizzes)
            {
                if (record.UserId == quizRecord.UserId && record.QuizId == quizRecord.QuizId)
                {
                    currentRecord = record;
                    break;
                }
                index++;
            }
            if (currentRecord == null)
            {
                user.AttemptedQuizzes = user.AttemptedQuizzes.Append(quizRecord);
                _userRepository.Update(user);
            }
            else
            {
                if (quizRecord.Score > currentRecord.Score ||
                    (quizRecord.Score == currentRecord.Score &&
                    quizRecord.LowestTime < currentRecord.LowestTime))
                {
                    List<QuizRecord> records = new List<QuizRecord>(user.AttemptedQuizzes);
                    records[index] = quizRecord;
                    user.AttemptedQuizzes = records;
                    _userRepository.Update(user);
                }
            }
        }
        public IEnumerable<User> GetOtherUsersExcept(int userId, int amount = 10)
        {
            return _userRepository.GetAll().Where(u => u.Id != userId).Take(amount);
        }

        public User GetUser(int id)
        {
            return _userRepository.GetWithId(id);
        }
        public User GetUserWithUsername(string username)
        {
            return _userRepository.GetWithUsername(username);
        }

        public User CreateUser(User user)
        {
            int userId = _userRepository.GetAll().Select(u => u.Id).Max() + 1;
            return _userRepository.Create(new User(userId, user.Username, user.Password));
        }
        public string GetUsernameWithId(int id)
        {
            return _userRepository.GetWithId(id).Username;
        }
    }
}
