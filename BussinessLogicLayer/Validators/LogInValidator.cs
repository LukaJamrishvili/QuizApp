using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace QuizApp
{
    public class LogInValidator
    {
        private readonly IUserRepository _userRepository;

        public LogInValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool CheckUser(string username, string password)
        {
            User user = _userRepository.GetWithUsername(username);
            if (user == null)
            {
                return false;
            }
            
            if (user.Password == password)
            {
                return true;
            }
            return false;
        }
    }
}
