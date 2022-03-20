namespace WebApp.Domain;

public class Question:BaseEntity
{
    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }
    
    public string QuestionName { get; set; } = default!;
    public int TimesAnswered { get; set; }
    public int? TimesAnsweredCorrect { get; set; }
    
    public ICollection<Answer>? Answers { get; set; }

}