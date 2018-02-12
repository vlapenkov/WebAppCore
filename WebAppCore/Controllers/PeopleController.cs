using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppCore.Data;
using WebAppCore.Models;
using WebAppCore.Services;


// dev
namespace WebAppCore.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly DbCachingService _dbCs;

        public PeopleController(ApplicationDbContext context, DbCachingService cachingService)
        {
            _context = context;
            _dbCs = cachingService;
        }

        // GET: People

        [ResponseCache(Duration = 100)]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persons.ToListAsync());
        }


        /// <summary>
        /// Example of paginatedlist
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchstring"></param>
        /// <returns></returns>
        /// Пример списка с пагинацией. Работает с отборами и без! проверял.
        [ResponseCache(Duration = 100, VaryByQueryKeys =new []{"page", "searchstring"})]
        public async Task<IActionResult> List(int? page,string searchstring)
        {

            int pageSize = 3;
            IQueryable <Person> persons = _context.Persons.Where(p => searchstring == null || p.Name.Contains(searchstring)).AsNoTracking();
            ViewData["CurrentFilter"] = searchstring;

           return View(await PaginatedList<Person>.CreateAsync(persons/*_context.Persons.AsNoTracking()*/, page ?? 1, pageSize));
        }


        public IActionResult GetFirst()
        {

            return Content($"First person is ={_dbCs.FirstPerson.Name}");
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,IsMarried,IsAutoCreated,TestProp1")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,IsMarried,IsAutoCreated,TestProp1")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.SingleOrDefaultAsync(m => m.Id == id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
