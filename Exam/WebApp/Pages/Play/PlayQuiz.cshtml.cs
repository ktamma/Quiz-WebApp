using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Domain;

namespace WebApp.Pages.Play
{
    public class PlayQuizModel : PageModel
    {
        private readonly ILogger<PlayQuizModel> _logger;
        private readonly DAL.AppDbContext _context;

        public Quiz? Quiz { get; set; }
        public Question? Question { get; set; }

        [BindProperty] public int? QuizID { get; set; }
        [BindProperty] public int? NextQuestion { get; set; }
        [BindProperty] public int? GameID { get; set; }
        [BindProperty] public int? Answer { get; set; }
        public int id { get; set; }
        public int n { get; set; }

        public int? game = null;

        public IList<Question>? Questions { get; set; }


        public PlayQuizModel(DAL.AppDbContext context, ILogger<PlayQuizModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task OnGetAsync(int id,int n,int? game)
        {

            this.id = id;
            this.n = n;
            this.game = game;

            Quiz = await _context.Quizzes
           .Where(x => x.Id == this.id).FirstOrDefaultAsync();

            Questions = await _context.Questions.Where(x=> x.QuizId == this.id).ToListAsync();

            try
            {
                Question = await _context.Questions.Where(x => x.Id == Questions[this.n - 1].Id).Include(x => x.Answers).FirstOrDefaultAsync();

            }
            catch (Exception e)
            {
                Question = new Question()
                {
                    Answers = new List<Answer>(),
                    TimesAnswered = 0,
                    QuestionName = "Empty question",
                    TimesAnsweredCorrect = 0
                };
            }



        }

        public async Task<IActionResult> OnPostAsync()
        {
            Questions = await _context.Questions.Where(x => x.QuizId == QuizID).ToListAsync();
            Question = await _context.Questions.Where(x => x.Id == Questions[(int)NextQuestion! - 2].Id).Include(x => x.Answers).FirstOrDefaultAsync();

            var dict = new Dictionary<int, int>();
            var Game = new PlayQuiz();
            dict.Add(Question!.Id,(int)Answer!);

            var answer = _context.Answers.FirstOrDefault(a => a.Id == Answer);
            if (answer!.IsCorrect == 1)
            {
                Question.TimesAnsweredCorrect += 1;
            }
            answer.TimesSelected += 1;
            Question.TimesAnswered += 1;
            _context.Attach(Question).State = EntityState.Modified;
            
            _context.Attach(answer).State = EntityState.Modified;
            _context.SaveChanges();
            if(GameID == null)
            {
                 Game = new PlayQuiz
                {
                    QuizId = (int)QuizID!,
                    Answers = JsonConvert.SerializeObject(dict)
                };

                _context.PlayQuizzes.Add(Game);
                await _context.SaveChangesAsync();
                Game = await _context.PlayQuizzes.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                GameID = Game!.Id;
            }
            else
            {
                Game = await _context.PlayQuizzes.Where(x => x.Id == GameID).FirstOrDefaultAsync();
                dict = JsonConvert.DeserializeObject<Dictionary<int,int>>(Game!.Answers);
                dict!.Add(Question.Id, (int)Answer!);
                Game.Answers = JsonConvert.SerializeObject(dict);
                _context.PlayQuizzes.Update(Game);
                await _context.SaveChangesAsync();
            }

            if (Questions.Count < NextQuestion) {
                return RedirectToPage("./Results",new {game = GameID });
            }
            return RedirectToPage("./PlayQuiz",new { id = QuizID,n=NextQuestion,game = GameID});
        }
    }
}
