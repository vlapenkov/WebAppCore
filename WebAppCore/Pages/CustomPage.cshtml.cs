using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.DAL;


namespace WebAppCore
{
    public class CustomPageModel : PageModel
    {
        private readonly ApplicationDbContext _appDbContext;

        public CustomPageModel(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
       
        public string Message { get; private set; } = "PageModel in C#";

        public string FirstPersonName { get; set; }

        public void OnGet(string param1, int? page)
        {
            Message += $" Server time is { DateTime.Now } ,param1 is:{param1} , page is : {page}";

            FirstPersonName = _appDbContext.Persons.First()?.Name;
        }
    }
}