using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DAL;
using X.PagedList;

namespace WebAppCore.Models
{
    public class PersonsViewModel
    {
        public IPagedList<Person> PersonsList { get; set; }
        public int Page { get; set; } = 1;
        public string SearchString { get; set; }

    }
}
