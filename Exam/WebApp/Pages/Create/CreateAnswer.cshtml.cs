
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebApp.Domain;

namespace WebApp.Pages.Create
{
    public class CreateAnswerModel : PageModel
    {
        private readonly ILogger<CreateAnswerModel> _logger;
        private readonly DAL.AppDbContext _context;

        [BindProperty] public string Text { get; set; } = default!;
        [BindProperty] public int Correct { get; set; } = default!;
        [BindProperty] public int QuestionId { get; set; } = default!;
        [BindProperty] public Question Question { get; set; } = default!;

        public int? Id { get; set; }
        public CreateAnswerModel(DAL.AppDbContext context, ILogger<CreateAnswerModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void OnGet(int id)
        {
            this.Id = id;
            Question = _context.Questions.FirstOrDefault(x => x.Id == id)!;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(Correct != 1)
            {
                Correct = 0;
            }

            var answer = new Answer()
            {
                AnswerName = Text,
                IsCorrect = Correct,
                QuestionId = QuestionId

            };
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Edit/EditQuestion", new { id = QuestionId });
        }

    }
}

