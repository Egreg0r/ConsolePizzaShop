using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPizzaShop.Models;

namespace WebPizzaShop.Controllers
{
    public class ChecksController : Controller
    {
        private readonly BaseContent _context;
        public ChecksController(BaseContent context)
        {
            _context = context;
        }

        // GET: Checks
        public async Task<IActionResult> Index()
        {
            var baseContent = _context.Checks
                .Include(c => c.Client)
                .Include(c => c.Orders)
                .AsNoTracking();
            return View(await baseContent.ToListAsync());
        }

        // GET: Checks/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Checks
                .Include(c => c.Orders)
                    .ThenInclude(ord => ord.Pizza)
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (check == null)
            {
                return NotFound();
            }
            var s = check.Orders.Sum(ord => ord.Pizza.Price).ToString();
            
            //вывод суммы с переводом в рубли.
            ViewData["SumCheck"] = s.Insert(s.Length - 2, ",");
            ViewBag.pizzas = Pizza.ListPizzasInCheck(check.Id, _context);
            return View(check);
        }

        // GET: Checks/Create
        public IActionResult Create()
        {
            var check = _context.Checks
               .Include(c => c.Orders)
                   .ThenInclude(ord => ord.Pizza)
               .Include(c => c.Client);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email");
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Name");
            return View();
        }

        // POST: Checks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,Adress,CreateDate,Paid,CloseDate,Guid,Id")] Check check)
        {
            if (ModelState.IsValid)
            {
                if (check.Paid == true)
                    check.CloseDate = DateTime.Now;
                _context.Add(check);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", check.ClientId);
            return View(check);
        }

        // GET: Checks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Checks.FindAsync(id);
            if (check == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", check.ClientId);
            return View(check);
        }

        // POST: Checks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,Adress,CreateDate,Paid,CloseDate,Guid,Id")] Check check)
        {
            if (id != check.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(check);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckExists(check.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Email", check.ClientId);
            return View(check);
        }

        private bool CheckExists(int id)
        {
            return _context.Checks.Any(e => e.Id == id);
        }

        // Get



    }

}
