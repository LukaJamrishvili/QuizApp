
namespace DataAccessLayer.Entities
{
    public class QuizQuestion : BaseEntity
    {
        public QuizQuestion() : base(0) { }
        public QuizQuestion(int quizId, string question, IEnumerable<string> answers, int correctAnswer) : base(0)
        {
            QuizId = quizId;
            Question = question;
            Answers = answers;
            CorrectAnswer = correctAnswer;
        }
        public int QuizId { get; set; }
        public string Question { get; set; }
        public IEnumerable<string> Answers { get; set; }
        public int CorrectAnswer { get; set; }
    }
}
