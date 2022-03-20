
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Domain;

namespace WebApp.Pages.Edit
{
    public class EditAnswerModel : PageModel
    {
        private readonly ILogger<EditAnswerModel> _logger;
        private readonly DAL.AppDbContext _context;

        public int? id { get; set; }
        [BindProperty] public string AnswerName { get; set; } = default!;
        [BindProperty] public int Correct { get; set; } = default!;
        [BindProperty] public int AnswerId { get; set; } = default!;
        [BindProperty] public int QuestionId { get; set; } = default!;
        public Answer? Answer { get; set; }

        public bool corr = false;

        public ICollection<Question>? Questions { get; set; }


        public EditAnswerModel(DAL.AppDbContext context, ILogger<EditAnswerModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync(int id)
        {

            this.id = id;
            Answer = await _context.Answers
            .Where(x => x.Id == id).OrderBy(x => x.AnswerName)
            .LastOrDefaultAsync();
            if(Answer!.IsCorrect == 1)
            {
                corr = true;
            }
            Questions = await _context.Questions.ToListAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Answer = await _context.Answers
            .Where(x => x.Id == AnswerId).OrderBy(x => x.AnswerName)
            .LastOrDefaultAsync();

            if (Correct != 1)
            {
                Correct = 0;
            }

            Answer!.AnswerName = AnswerName;
            Answer.IsCorrect = Correct;         
            _context.Attach(Answer).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToPage("../Edit/EditQuestion", new { id = QuestionId });
        }
    }
}
