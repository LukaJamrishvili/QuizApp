using DataAccessLayer.Interfaces;

namespace QuizApp
{
    public class RegistrationValidator
    {
        private readonly IUserRepository _userRepository;

        public RegistrationValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public static void CustomColorWrite(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        public bool CheckUsername(string username)
        {
            if (5 > username.Length || username.Length > 15)
            {
                CustomColorWrite(ConsoleColor.Red, "Invalid length! Try another!\n");
                Console.WriteLine("Press any key to continue...");
                return false;
            }
            if (_userRepository.Contains(username))
            {
                CustomColorWrite(ConsoleColor.Red, "This username is already used! Try another!\n");
                Console.WriteLine("Press any key to continue...");
                return false;
            }
            CustomColorWrite(ConsoleColor.Green, "_________________________________\n\n");
            return true;
        }

        public bool CheckPassword(string password)
        {
            if (8 > password.Length || password.Length > 18)
            {
                return false;
            }

            CustomColorWrite(ConsoleColor.Green, "_________________________________\n\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return true;

        }
    }
}
