namespace QuizApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string userConStr = "C:\\Users\\Lukee_219\\Desktop\\QuizAppForItStep\\QuizApp\\DatabaseLayer\\JsonBasedDatabase\\Users.json";
            string quizConStr = "C:\\Users\\Lukee_219\\Desktop\\QuizAppForItStep\\QuizApp\\DatabaseLayer\\JsonBasedDatabase\\Quizzes.json";
            UserInteface userInteface = new UserInteface(userConStr, quizConStr);
            userInteface.MainMenu();
        }
    }
}