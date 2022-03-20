#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.DAL;
using WebApp.Domain;

namespace WebApp.Pages_Quizzes
{
    public class DeleteModel : PageModel
    {
        private readonly WebApp.DAL.AppDbContext _context;

        public DeleteModel(WebApp.DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Quiz = await _context.Quizzes.FirstOrDefaultAsync(m => m.Id == id);

            if (Quiz == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Quiz = await _context.Quizzes.FindAsync(id);

            if (Quiz != null)
            {
                List<Question> questionsList = _context.Questions.Where(s => s.QuizId == Quiz.Id).ToList();
                foreach (var question in questionsList)
                {
                    List<Answer> answerslist = _context.Answers.Where(a => a.QuestionId == question.Id).ToList();

                    foreach (var answer in answerslist)
                    {
                        _context.Answers.Remove(answer);

                    }
                    _context.Questions.Remove(question);
                }
                _context.Quizzes.Remove(Quiz);
                
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
