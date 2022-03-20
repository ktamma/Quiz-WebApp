
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Domain;

namespace WebApp.Pages.Create
{
    public class CreateQuestionModel : PageModel
    {
        private readonly ILogger<CreateQuestionModel> _logger;
        private readonly DAL.AppDbContext _context;

        [BindProperty] public string Name { get; set; } = default!;
        [BindProperty] public int QuizId { get; set; } = default!;
        [BindProperty] public Quiz Quiz { get; set; } = default!;


        public int? id { get; set; }

        public CreateQuestionModel(DAL.AppDbContext context, ILogger<CreateQuestionModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnGet(int id)
        {
            this.id = id;
            Quiz = _context.Quizzes.FirstOrDefault(q => q.Id == id)!;

        }

        public async Task<IActionResult> OnPostAsync()
        {

            var question = new Question
            {
                QuizId = QuizId,
                QuestionName = Name,
                TimesAnswered = 0,
                TimesAnsweredCorrect = 0
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Edit/EditQuiz", new {id = QuizId });
        }

    }
}
