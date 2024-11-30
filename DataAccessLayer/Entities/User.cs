namespace DataAccessLayer.Entities
{
    public class User : BaseEntity
    {
        public User(int id, string username, string password) : base(id)
        {
            Username = username;
            Password = password;
            AttemptedQuizzes = new List<QuizRecord>();
        }
        public int Id { get => _id; }
        public int Score { get
            {
                if (AttemptedQuizzes.Count() == 0)
                {
                    return 0;
                }
                else
                {
                    return AttemptedQuizzes.Sum(a => a.Score);
                }
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public IEnumerable<QuizRecord> AttemptedQuizzes { get; set; }
    }
}
