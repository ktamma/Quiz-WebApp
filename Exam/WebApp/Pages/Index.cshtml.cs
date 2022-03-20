using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Domain;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DAL.AppDbContext _context;

    public IndexModel(DAL.AppDbContext context, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _context = context;
    }

    public List<Quiz>? Quiz { get; set; }

    public IList<Quiz> Quizzes { get; set; } = default!;

    public string? SearchName { get; set; }

    [BindProperty] public int? DeleteID { get; set; }

    public async Task OnGetAsync(string? searchName, string action)
    {
        Quiz = await _context.Quizzes.ToListAsync();

        Quizzes = _context.Quizzes.OrderBy(x => x.Id).Reverse().ToList();


        if (action == "Clear")
        {
            searchName = null;
        }

        SearchName = searchName;

        if (!string.IsNullOrWhiteSpace(searchName))
        {
            searchName = searchName.Trim();
        }

        Console.WriteLine(searchName);
        if (searchName != null)
        {
            Quizzes = Quizzes.Where(q => q.Name.ToUpper().Contains(searchName.ToUpper())).ToList();
        }

        if (Quizzes.Count == 0)
        {
            if (searchName != null)
            {
                Quizzes = Quiz.Where(q => q.Description.ToUpper().Contains(searchName.ToUpper())).ToList();
            }
        }
    }

    public void OnPost(int? id)
    {
        DeleteID = id;
        Console.WriteLine(id);
        if (DeleteID != null)
        {
            Console.WriteLine(DeleteID);
            var toDelete = _context!.Quizzes.Where(x => x.Id == DeleteID).FirstOrDefault();

            var questions = _context!.Questions.Where(x => x.QuizId == DeleteID).ToList();

            var questionId = questions.First().Id;
            var answers = _context!.Answers.Where(x => x.QuestionId == questionId).ToList();

            foreach (var answer in answers)
            {
                _context.Answers.Remove(answer);
            }

            _context.SaveChangesAsync();
            foreach (var question in questions)
            {
                _context.Questions.Remove(question);
            }

            _context.SaveChangesAsync();

            _context.Quizzes.Remove(toDelete!);

            _context.SaveChangesAsync();
        }
    }
}