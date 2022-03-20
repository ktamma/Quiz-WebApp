using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Domain;

namespace WebApp.Pages.Statistics
{
    public class QuizStatisticsModel : PageModel
    {
        public void OnGet(int id)
        {
            
            
        }
        
        
        public Quiz? Quiz { get; set; }

    }
}
