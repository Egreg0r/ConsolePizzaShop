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
    public class ClientsController : Controller
    {
        private readonly BaseContent _context;

        public ClientsController(BaseContent context)
        {
            _context = context;
        }


        #region Index

        // GET: Clients
        public async Task<IActionResult> Index()
        {

            return View(await _context.Clients.OrderBy(p => p.Name).ToListAsync());
        }

        // GET: Clients/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            // чек и его сумма
            var checkWhithSum = new List<(string id, string date, string adress, string sum)>();
            int credit = 0;
            foreach (var ch in await GetClientNoPaidCheck(client.Id))
            {
                var k = ch.SumCheck(ch.Id);
                credit = credit + k;
                checkWhithSum.Add((ch.Id.ToString(), ch.CreateDate.ToString("dd.MM.yyyy hh:mm"), ch.Adress, k.IntToRub()));
            }

            ViewData["Credit"] = credit.IntToRub();
            if (!checkWhithSum.Any()) ViewData["Disabled"] = "disabled";
            ViewBag.cheks = checkWhithSum;
            return View(client);
        }

        [HttpPost, ActionName("Paid")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Paid(int id)
        {
            foreach (var p in await GetClientNoPaidCheck(id))
            {
                SetChecksIsPaid(p.Id);
            }
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Create

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,RegistrDate,Guid,Id")] Client client)
        {
            if (ModelState.IsValid)
            {
                client.RegistrDate = DateTime.Now;
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
        #endregion

        #region Edit
        // GET: Clients/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Email,RegistrDate,Guid,Id")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        #endregion

        #region Delete

        // GET: Clients/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        // Проверка на  уникальность Email
        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email)
        {
            if (_context.Clients.Where(cl => cl.Email == email).Any())
                return Json(false);
            return Json(true);
        }

        #region method

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

        /// <summary>
        /// Проставление признака оплаты чека. 
        /// </summary>
        /// <param name="id">Check.id</param>
        public async void SetChecksIsPaid(int id)
        {
            using (var db = new BaseContent())
            {
                Check check = db.Checks.FirstOrDefault(c => c.Id == id);
                if (check != null)
                {
                    check.Paid = true;
                    check.CloseDate = DateTime.Now;
                    await db.SaveChangesAsync();
                }
            }
        }


        /// <summary>
        /// Возвращает список неоплаченных счетов
        /// </summary>
        /// <param name=" clienId">Client.id</param>

        public async Task<ICollection<Check>> GetClientNoPaidCheck(int id)
        {
            using (var db = new BaseContent())
            {
                var check = db.Checks
                    .Include(c => c.Orders)
                    .Where(cl => cl.ClientId == id && cl.Paid == false)
                    .AsNoTracking();
                return await check.ToListAsync();
            }

        }
        #endregion
    }
}


