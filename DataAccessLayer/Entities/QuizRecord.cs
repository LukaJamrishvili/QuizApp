namespace DataAccessLayer.Entities
{
    public class QuizRecord : BaseEntity
    {
        public QuizRecord() : base(0) { }
        public QuizRecord(string username, int score, int lowestTime, int quizId = 0, int userId = 0) : base(0)
        {
            Username = username;
            Score = score;
            LowestTime = lowestTime;
            QuizId = quizId;
            UserId = userId;
        }
        public int QuizId {  get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public int LowestTime { get; set; }
    }
}
