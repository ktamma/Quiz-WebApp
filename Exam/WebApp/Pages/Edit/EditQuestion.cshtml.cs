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
    public class EditQuestionModel : PageModel
    {
        private readonly ILogger<EditQuestionModel> _logger;
        private readonly DAL.AppDbContext _context;

        public int? id { get; set; }
        public Question? Question { get; set; }

        public ICollection<Answer>? Answers { get; set; }
        [BindProperty] public int? DeleteID { get; set; }
        [BindProperty] public int? ReturnID { get; set; }


        public EditQuestionModel(DAL.AppDbContext context, ILogger<EditQuestionModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync(int id)
        {

            this.id = id;
            Question = await _context.Questions
            .Where(x => x.Id == id).OrderBy(x => x.QuestionName)
            .LastOrDefaultAsync();

            Answers = await _context.Answers.Where(x => x.QuestionId == this.id).ToListAsync();
            
        }

        public async Task<RedirectToPageResult> OnPostAsync()
        {
            {
                if (DeleteID != null)
                {

                    var toDelete = await _context.Answers.Where(x => x.Id == DeleteID).FirstOrDefaultAsync();

                    _context.Answers.Remove(toDelete!);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./EditQuestion",new {id = ReturnID });
                }
                return RedirectToPage("../Index");
            }
        }
    }
}
