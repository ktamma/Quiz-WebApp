using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class Quiz:BaseEntity
{
    [MaxLength(64)]
    public string Name { get; set; } = default!;
    
    public string Description { get; set; } = default!;
    
    public int TimesCompleted { get; set; } = default!;
    
    public ICollection<Question>? Questions { get; set; }

}