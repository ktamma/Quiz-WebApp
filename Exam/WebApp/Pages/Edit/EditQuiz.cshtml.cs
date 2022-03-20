using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Domain;

namespace WebApp.Pages.Edit
{
    public class EditQuizModel : PageModel
    {

        private readonly ILogger<EditQuizModel> _logger;
        private readonly DAL.AppDbContext _context;

        public int? id { get; set; }
        public Quiz? Quiz { get; set; }

        public ICollection<Question>? Questions { get; set; }

        [BindProperty] public int? DeleteID { get; set; }
        [BindProperty] public int? ReturnID { get; set; }


        public EditQuizModel(DAL.AppDbContext context, ILogger<EditQuizModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync(int id)
        {

            this.id = id;
            Quiz = await _context.Quizzes
            .Where(x => x.Id == id).OrderBy(x => x.Id)
            .LastOrDefaultAsync();

            Questions = await _context.Questions.Where(x => x.QuizId == id).ToListAsync();
            
        }

        public async Task<RedirectToPageResult> OnPostAsync()
        {
            {
                if (DeleteID != null)
                {

                    var toDelete = await _context.Questions.Where(x => x.Id == DeleteID).FirstOrDefaultAsync();

                    _context.Questions.Remove(toDelete!);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./EditQuiz", new { id = ReturnID });
                }
                return RedirectToPage("../Index");
            }

        }
    }
}
