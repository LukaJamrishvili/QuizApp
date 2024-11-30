using BussinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using System.Diagnostics;

namespace QuizApp
{
    public class UserInteface
    {
        private readonly AccountService _accountService;
        private readonly UserService _userService;
        private int _userId = -1;


        public UserInteface(string userConnectionString, string quizConnectionString)
        {
            _accountService = new AccountService(_userId, new QuizRepository(quizConnectionString));
            _userService = new UserService(new UserRepository(userConnectionString));
        }

        private static int MenuDisplay(string title, string[] options)
        {
            Console.Clear();
            CustomColorWrite(ConsoleColor.Yellow, $"=== {title} ===\n\n");
            foreach (string option in options)
            {
                CustomColorWrite(ConsoleColor.Cyan, $"{option}\n");
            }
            Console.Write("\nInput: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                return choice;
            }

            Console.WriteLine("Invalid input! Please enter a number.");
            Console.ReadKey();
            return MenuDisplay(title, options);
        }
        public static void CustomColorWrite(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }


        public void MainMenu()
        {
            string[] options = { "1. Register.", "2. Log in.", "3. Check top users.", "4. Exit." };
            while (true)
            {
                int option = MenuDisplay("Main Menu", options);
                switch (option)
                {
                    case 1:
                        RegistrationInterface();
                        break;
                    case 2:
                        LogInInterface();
                        break;
                    case 3:
                        CheckTopUsers();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        MainMenu();
                        break;
                }
            }

        }

        private void RegistrationInterface()
        {
            string username;
            string password;

            while (true)
            {
                Console.Clear();
                CustomColorWrite(ConsoleColor.Yellow, $"=== Registration ===\n\n");
                CustomColorWrite(ConsoleColor.Cyan, "Enter your username (must be 5-15 characters): ");
                username = Console.ReadLine();
                if (_userService._registrationValidator.CheckUsername(username))
                {
                    break;
                }
                Console.ReadKey();
            }
            while (true)
            {
                CustomColorWrite(ConsoleColor.Cyan, "Enter your password (must be 8-18 characters): ");
                password = Console.ReadLine();
                if (_userService._registrationValidator.CheckPassword(password))
                {
                    break;
                }
                else
                {
                    CustomColorWrite(ConsoleColor.Red, "Invalid length! Try another!\n");
                    Console.WriteLine("Press any key to continue...");
                }
                Console.ReadKey();
                Console.Clear();
                CustomColorWrite(ConsoleColor.Yellow, $"=== Registration ===\n\n");
            }
            _userService.CreateUser(new User(0, username, password));
            Console.Clear();
            CustomColorWrite(ConsoleColor.Green, "Successfully registered!\n\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        private bool LogInInterface()
        {
            Console.Clear();
            CustomColorWrite(ConsoleColor.Yellow, $"=== Log In ===\n\n");

            CustomColorWrite(ConsoleColor.Cyan, "Enter your username: ");
            string username = Console.ReadLine();
            CustomColorWrite(ConsoleColor.Cyan, "Enter your password: ");
            string password = Console.ReadLine();

            if (!_userService._logInValidator.CheckUser(username, password))
            {
                CustomColorWrite(ConsoleColor.Red, "Invalid credentials! Try Again!");
                Console.ReadLine();
                Console.Clear();
                return false;
            }

            _userId = _userService.GetUserWithUsername(username).Id;

            CustomColorWrite(ConsoleColor.Green, "\nSuccessfully logged in!\n");
            Console.ReadKey();
            Console.Clear();
            AccountMenu();
            return true;
        }

        private void CheckTopUsers()
        {
            string[] options = _userService.GetAllUsers()
                .OrderByDescending(u => u.Score)
                .Select(u => $"{u.Username} - {u.Score}")
                .Take(10).Append("Exit.").ToArray();
            options[options.Length - 1] = $"{options.Length}. Exit.";

            while (true)
            {
                int option = MenuDisplay("Top 10 users", options);
                if (option == options.Length)
                {
                    return;
                }
            }
        }

        private void AccountMenu()
        {
            string[] options = {
                "1. Create new quiz.",
                "2. Your quizzes.",
                "3. Search other quizzes with username.",
                "4. Log out.",
            };
            while (true)
            {
                int option = MenuDisplay("Account Menu", options);
                switch (option)
                {
                    case 1:
                        QuizCreationInterface();
                        break;
                    case 2:
                        QuizLoadingInterface(_userId);
                        break;
                    case 3:
                        QuizSearchingInterface();
                        break;
                    case 4:
                        _userId = -1;
                        return;
                    default:
                        break;
                }
            }
        }

        private void QuizCreationInterface()
        {
            Console.Clear();
            CustomColorWrite(ConsoleColor.Yellow, $"=== Create Quiz ===\n\n");

            CustomColorWrite(ConsoleColor.Cyan, "Enter title: ");
            string title = Console.ReadLine();
            Quiz newQuiz = new Quiz(0, _userId, title, null);
            QuizQuestionCreationInterface(newQuiz);
            _accountService.CreateQuiz(newQuiz);
        }

        private void QuizQuestionCreationInterface(Quiz quiz)
        {
            Console.Clear();
            QuizQuestion[] quizQuestions = new QuizQuestion[5];
            for (int i = 0; i < 5; i++)
            {
                Console.Clear();
                CustomColorWrite(ConsoleColor.Yellow, $"=== Question {i + 1} ===\n\n");


                CustomColorWrite(ConsoleColor.Cyan, $"Enter question ({i + 1}): ");

                string question = Console.ReadLine();
                string[] answers = new string[4];

                Console.Clear();
                CustomColorWrite(ConsoleColor.Yellow, $"=== {question} ===\n\n");
                for (int j = 0; j < 4; j++)
                {
                    CustomColorWrite(ConsoleColor.Cyan, $"Enter answer ({j + 1}): ");
                    answers[j] = Console.ReadLine();
                }

                CustomColorWrite(ConsoleColor.Green, "\nWhich answer is correct (1 - 4): \n");
                int correctAnswer;
                bool isParsed = int.TryParse(Console.ReadLine(), out correctAnswer);
                correctAnswer = !isParsed ? 1 : (correctAnswer > 4 && correctAnswer < 1) ? 1 : correctAnswer;

                quizQuestions[i] = new QuizQuestion()
                {
                    QuizId = quiz.Id,
                    Question = question,
                    Answers = answers,
                    CorrectAnswer = correctAnswer,
                };
            }

            quiz.Questions = quizQuestions;
            Console.ReadKey();

        }

        private void QuizLoadingInterface(int userId)
        {
            List<Quiz> quizzes = _accountService.GetQuizzesWithUserId(userId).Take(10).ToList();
            string[] options = quizzes.Select(q => q.QuizTitle).Append("Exit").ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = $"{i + 1}. {options[i]}.";
            }
            while (true)
            {
                int option = MenuDisplay("Quiz titles", options);
                if (option == options.Length)
                {
                    return;
                }
                if (0 < option && option <= options.Length)
                {
                    if (userId == _userId)
                    {
                        if (ReadUpdateDeleteInterface(quizzes[option - 1]))
                        {
                            return;
                        }
                    }
                    else
                    {
                        AttemptInterface(quizzes[option - 1]);
                    }
                }
            }
        }

        private void AttemptInterface(Quiz quiz)
        {
            string[] options = { "1. Start Quiz.", "2. View Record.", "3. Exit." };
            while (true)
            {
                int option = MenuDisplay(quiz.QuizTitle, options);
                switch (option)
                {
                    case 1:
                        QuizStartInterface(quiz);
                        break;
                    case 2:
                        RecordViewingInterface(quiz);
                        break;
                    case 3:
                        return;
                    default:
                        break;
                }
            }
        }

        private bool ReadUpdateDeleteInterface(Quiz quiz)
        {
            string[] options = { "1. View.", "2. Update.", "3. Delete.", "4. Exit." };
            while (true)
            {
                int option = MenuDisplay(quiz.QuizTitle, options);
                switch (option)
                {
                    case 1:
                        QuizViewingInterface(quiz);
                        break;
                    case 2:
                        QuizUpdatingInterface(quiz);
                        break;
                    case 3:
                        _accountService.DeleteQuiz(quiz);
                        return true;
                    case 4:
                        return false;
                    default:
                        break;
                }
            }
        }

        private void QuizViewingInterface(Quiz quiz)
        {
            string[] options = quiz.Questions.Select(q => q.Question).Append("Exit.").ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = $"{i + 1}. {options[i]}";
            }
            while (true)
            {
                int option = MenuDisplay("View Question", options);
                if (option == options.Length)
                {
                    return;
                }
                if (0 < option && option <= options.Length)
                {
                    QuizViewingInterface(quiz.Questions.ToArray()[option - 1]);
                }
            }
        }

        private void QuizViewingInterface(QuizQuestion quizQuestion)
        {
            string[] options = quizQuestion.Answers.Append("Correct answer - ").Append("Exit").ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                if (i == options.Length - 2)
                {
                    options[i] = $"\n{options[i]}{quizQuestion.CorrectAnswer}.\n";
                    continue;
                }
                options[i] = $"{i + 1}. {options[i]}.";
            }
            while (true)
            {
                int option = MenuDisplay(quizQuestion.Question, options);
                if (option == options.Length)
                {
                    return;
                }
            }
        }

        private void QuizUpdatingInterface(Quiz quiz)
        {
            string[] options = quiz.Questions.Select(q => q.Question).Append("Exit").ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = $"{i + 1}. {options[i]}.";
            }
            while (true)
            {
                int option = MenuDisplay("Update Question", options);
                if (option == options.Length)
                {
                    return;
                }
                if (0 < option && option <= options.Length)
                {
                    if (QuizUpdate(quiz, option - 1))
                    {
                        return;
                    }
                }
            }

        }

        private bool QuizUpdate(Quiz quiz, int questionIndex)
        {
            string[] options = quiz.Questions.ToArray()[questionIndex]
                .Answers.Append("Title").Append("Correct answer")
                .Append("Exit").ToArray();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == 3)
                {
                    options[i] = $"{i + 1}. {options[i]}.\n__________\n";
                    continue;
                }
                options[i] = $"{i + 1}. {options[i]}.";
            }

            bool update = false;
            while (true)
            {
                int option = MenuDisplay("Choose one to update", options);
                if (option == options.Length)
                {
                    return false;
                }
                else if (0 < option && option <= 4)
                {
                    string newAnswer = QuizUpdatingInterface($"Write answer {option}", "Enter new answer: ");
                    string[] answers = quiz.Questions.ToArray()[questionIndex].Answers.ToArray();
                    answers[option - 1] = newAnswer;
                    quiz.Questions.ToArray()[questionIndex].Answers = answers;
                    update = true;
                }
                else if (option == options.Length - 2)
                {
                    string newQuestion = QuizUpdatingInterface("Write new question", "Enter new question: ");
                    quiz.Questions.ToArray()[questionIndex].Question = newQuestion;
                    update = true;
                }
                else if (option == options.Length - 1)
                {
                    string newCorrectAnswer = QuizUpdatingInterface($"Writ correct answer", "Enter correct answer (1-4): ");

                    if (int.TryParse(newCorrectAnswer, out int parsed))
                    {
                        if (0 < parsed && parsed < 5)
                        {
                            quiz.Questions.ToArray()[questionIndex].CorrectAnswer = parsed;
                            update = true;
                        }
                    }
                }
                if (update)
                {
                    if (_accountService.UpdateQuiz(quiz))
                    {
                        CustomColorWrite(ConsoleColor.Green, "Successfully updated!\n");
                        Console.ReadLine();
                        return true;
                    }
                }
            }
        }

        private static string QuizUpdatingInterface(string title, string message)
        {
            Console.Clear();
            CustomColorWrite(ConsoleColor.Yellow, $"=== {title} ===\n\n");
            CustomColorWrite(ConsoleColor.Cyan, $"{message}");
            return Console.ReadLine();
        }

        private void QuizSearchingInterface()
        {
            List<User> users = _userService.GetOtherUsersExcept(_userId).ToList();
            string[] options = users.Select(q => q.Username).Append("Exit").ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = $"{i + 1}. {options[i]}.";
            }
            while (true)
            {
                int option = MenuDisplay("Usernames", options);
                if (option == options.Length)
                {
                    return;
                }
                if (0 < option && option <= options.Length)
                {
                    QuizLoadingInterface(users[option - 1].Id);
                }

            }

        }

        private void RecordViewingInterface(Quiz quiz)
        {
            string[] options = { "Username: NONE", "Score: NONE", "Lowest time: NONE", "4. Exit." };
            int lowestTime = quiz.QuizRecord.LowestTime;
            if (quiz.QuizRecord != null)
            {
                options[0] = $"Username: {quiz.QuizRecord.Username}";
                options[1] = $"Score: {quiz.QuizRecord.Score}";
                options[2] = $"Lowest time: {lowestTime / 1000}.{lowestTime % 1000} seconds";
            }
            while (true)
            {
                int option = MenuDisplay(quiz.QuizTitle, options);
                if (option == 4)
                {
                    return;
                }
            }
        }

        private void QuizStartInterface(Quiz quiz)
        {
            int score = 0;
            int timeTaken = 0;
            const int maxBound = 20;
            bool timeOut = false;

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Task countDown = new Task(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (true)
                {
                    if (stopwatch.ElapsedMilliseconds > maxBound * 1000)
                    {
                        stopwatch.Stop();
                        timeOut = true;
                        CustomColorWrite(ConsoleColor.Magenta, "\nTime Out!\n...");
                        break;
                    }

                    if (cts.IsCancellationRequested)
                    {
                        stopwatch.Stop();
                        timeTaken = (int)stopwatch.ElapsedMilliseconds;
                        break;
                    }
                }
            });
            countDown.Start();
            QuizQuestion[] questions = quiz.Questions.ToArray();
            for (int i = 0; i < questions.Length; i++)
            {
                string[] options = questions[i].Answers.ToArray();
                for (int j = 0; j < options.Length; j++)
                {
                    options[j] = $"{j + 1}. {options[j]}";
                }
                int option = MenuDisplay(questions[i].Question, options);

                if (timeOut)
                {
                    score = -(questions.Length * 20);
                    break;
                }

                if (option == questions[i].CorrectAnswer)
                {
                    score += 20;
                }
                else
                {
                    score -= 20;
                }
            }
            cts.Cancel();
            ConsoleColor color = score > 0 ? ConsoleColor.Green : ConsoleColor.Red;
            if (color == ConsoleColor.Green)
            {
                QuizRecord quizRecord = new QuizRecord(_userService.GetUsernameWithId(_userId), score, timeTaken, quiz.Id, _userId);
                User user = _userService.GetUser(_userId);
                _userService.UpdateQuizRecord(user, quizRecord);
                
                bool isRecord = _accountService.CheckRecord(quiz, quizRecord);
                if (isRecord)
                {
                    CustomColorWrite(color, "New Record\n");
                }
            }
            Console.Write("Your score - ");
            CustomColorWrite(color, $"{score}\n");
            Console.ReadKey();
        }
    }
}
