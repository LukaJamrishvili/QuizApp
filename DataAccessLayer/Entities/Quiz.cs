using DataAccessLayer.Entities;

namespace DataAccessLayer.Entities
{
    public class Quiz : BaseEntity
    {
        public Quiz(int id, int ownerId, string quizTitle, IEnumerable<QuizQuestion> questions) : base(id)
        {
            OwnerId = ownerId;
            QuizTitle = quizTitle;
            Questions = questions;
            QuizRecord = new QuizRecord("None", 0, 120000, id, 0);
        }
        public int Id { get => _id; }
        public int OwnerId { get; set; }
        public string QuizTitle { get; set; }
        public QuizRecord QuizRecord { get; set; }
        public IEnumerable<QuizQuestion>  Questions { get; set; }
    }

}
