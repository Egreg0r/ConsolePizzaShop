using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebPizzaShop.Models;
using WebPizzaShop.Data;

namespace WebPizzaShop.Controllers
{
    public class ChecksController : Controller
    {
        private readonly BaseContent _context;
        public ChecksController(BaseContent context)
        {
            _context = context;
        }

        #region Index

        // GET: Checks
        public async Task<IActionResult> Index()
        {
            var baseContent = _context.Checks
                .Include(c => c.Client)
                .Include(c => c.Orders)
                .OrderByDescending(c => c.CreateDate)
                .AsNoTracking();
            return View(await baseContent.ToListAsync());
        }
        #endregion

        #region Details

        // GET: Checks/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var check = await _context.Checks
                .Include(c => c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (check == null)
            {
                return NotFound();
            }
            
            List<(string, string)> pizzaz = new List<(string, string)>();
            var pizzaCollection = Pizza.ListPizzasInCheck(check.Id, _context);
            foreach (var p in pizzaCollection)
            {
                pizzaz.Add((p.Name, p.Price.IntToRub()));
            }
            
            //вывод суммы чека
            ViewData["SumCheck"] = pizzaCollection.Sum(p => p.Price).IntToRub();

            // Список пиц в чеке
            ViewBag.pizzas = pizzaz;
            return View(check);
        }
        #endregion

        #region Create
        // GET: Checks/Create
        public IActionResult Create()
        {
            var check = _context.Checks
               .Include(c => c.Orders)
                   .ThenInclude(ord => ord.Pizza)
               .Include(c => c.Client);
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Name");
            return View();
        }

        // POST: Checks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,Adress,CreateDate,Paid,CloseDate,Guid,Id")] Check check, string pizcol, [Bind("CheckId, PizzaId, Id, Guid")] Order order)
        {
            if (ModelState.IsValid && order != null && Client.CanPaid(check.ClientId))
            {
                check.CreateDate = DateTime.Now;
                if (check.Paid == true)
                    check.CloseDate = DateTime.Now;
                
                //Проверка на не пустой список заказа
                var pizzas = new List<int>();
                for (int i = 0; i < (int)Convert.ToInt32(pizcol); i++)
                {
                    pizzas.Add(order.PizzaId);
                }
                //Производим заказ если клиент может это сделать. 
                Check.addCheckNow(check.ClientId, pizzas, paid: check.Paid);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            
            
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", check.ClientId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Name", order.PizzaId);
            return View(check);
        }

        #endregion
        

        #region Edit
        // GET: Checks/Edit
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

        // POST: Checks/Edit
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
        #endregion

        #region method
        private bool CheckExists(int id)
        {
            return _context.Checks.Any(e => e.Id == id);
        }

        ///// <summary>
        ///// Генерация нового заказа
        ///// </summary>
        ///// <param name="client"></param>
        ///// <param name="pizzasId"></param>
        ///// <param name="isPaid"></param>
        //private void CreateCheck(BaseContent db, int clientID, List<int> pizzas, bool isPaid = false)
        //{
        //    using (db)
        //    {
        //        Client client = db.Clients.Find(clientID);
        //        Console.WriteLine("Client " + client.Name);
        //        //Проверка на не пустой список заказа
        //        if (!pizzas.Any()) return;

        //        //Производим заказ если клиент может это сделать. 
        //        if (client.CanPaid(clientID))
        //        {
        //            Check.addCheck(db, clientID, pizzas, paid: isPaid);
        //        }
        //    }
        //}

        #endregion


    }

}
