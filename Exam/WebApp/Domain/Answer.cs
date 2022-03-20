namespace WebApp.Domain;

public class Answer:BaseEntity
{
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
    
    public string AnswerName { get; set; } = default!;
    
    public int IsCorrect { get; set; } = default!;
    public int TimesSelected { get; set; }
}