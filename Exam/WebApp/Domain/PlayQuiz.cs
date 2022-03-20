

namespace WebApp.Domain
{
    public class PlayQuiz:BaseEntity
    {
        public int QuizId { get; set; }
        
        public string Answers { get; set; } = default!;
    }
}