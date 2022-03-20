
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Domain;

namespace WebApp.Pages.Play
{
    public class ResultsModel : PageModel
    {
        private readonly ILogger<ResultsModel> _logger;
        private readonly DAL.AppDbContext _context;

        public PlayQuiz? Game { get; set; }

        public Quiz? Quiz { get; set; }
        public List<Question>? Questions { get; set; }

        public List<Answer>? Answers { get; set; }
        public int id { get; set; }
        public Dictionary<int,int>? results { get; set; }

        public int? game = null;

        public int correct = 0;

 


        public ResultsModel(DAL.AppDbContext context, ILogger<ResultsModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void OnGet(int game)
        {
            Answers = new List<Answer>();
            this.game = game;
            Game = _context.PlayQuizzes.Where(x => x.Id == this.game).OrderBy(x=> x.Id).LastOrDefault();
            results = JsonConvert.DeserializeObject<Dictionary<int, int>>(Game!.Answers);
            Quiz = _context.Quizzes.Where(x => x.Id == Game.QuizId).OrderBy(x => x.Id).LastOrDefault();
            Quiz!.TimesCompleted += 1;
            // _context.Quizzes.Update(Quiz);
            _context.Attach(Quiz).State = EntityState.Modified;

            _context.SaveChanges();
            Questions = _context.Questions.Where(x => x.QuizId == Game.QuizId).ToList();
            foreach(var question in Questions)
            {
                Answers.AddRange(_context.Answers.Where(x=> x.QuestionId == question.Id).ToList());
            }
            foreach (var answer in Answers)
            {
                if (results!.ContainsValue(answer.Id) && answer.IsCorrect == 1)
                {
                    correct += 1;
                }
            }

        }
    }
}
