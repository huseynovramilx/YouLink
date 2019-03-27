using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;
using LinkShortener.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace LinkShortener.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area("Dashboard")]
    public class PayoutRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PayoutRequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Dashboard/PayoutRequests
        public async Task<IActionResult> Index()
        {
            string userId = _userManager.GetUserId(User);
            var applicationDbContext = _context.PayoutRequests.Where(p=>p.OwnerId==userId).Where(p => p.Paid).Include(p => p.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dashboard/PayoutRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payoutRequest = await _context.PayoutRequests
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payoutRequest == null)
            {
                return NotFound();
            }

            return View(payoutRequest);
        }

        // GET: Dashboard/PayoutRequests/Create
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Dashboard/PayoutRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Money,OwnerId,Paid")] PayoutRequest payoutRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payoutRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", payoutRequest.OwnerId);
            return View(payoutRequest);
        }

        // GET: Dashboard/PayoutRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payoutRequest = await _context.PayoutRequests.FindAsync(id);
            if (payoutRequest == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", payoutRequest.OwnerId);
            return View(payoutRequest);
        }

        // POST: Dashboard/PayoutRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Money,OwnerId,Paid")] PayoutRequest payoutRequest)
        {
            if (id != payoutRequest.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payoutRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayoutRequestExists(payoutRequest.ID))
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
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", payoutRequest.OwnerId);
            return View(payoutRequest);
        }

        // GET: Dashboard/PayoutRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payoutRequest = await _context.PayoutRequests
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payoutRequest == null)
            {
                return NotFound();
            }

            return View(payoutRequest);
        }

        // POST: Dashboard/PayoutRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payoutRequest = await _context.PayoutRequests.FindAsync(id);
            _context.PayoutRequests.Remove(payoutRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayoutRequestExists(int id)
        {
            return _context.PayoutRequests.Any(e => e.ID == id);
        }
    }
}
