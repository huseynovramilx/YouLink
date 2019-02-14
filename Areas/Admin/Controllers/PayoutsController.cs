using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;
using LinkShortener.Models;

namespace LinkShortener.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PayoutsController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public PayoutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Payouts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Payouts.Include(p => p.PayoutBatch);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Payouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payout = await _context.Payouts
                .Include(p => p.PayoutBatch)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payout == null)
            {
                return NotFound();
            }

            return View(payout);
        }

        // GET: Admin/Payouts/Create
        public IActionResult Create()
        {
            ViewData["PayoutBatchId"] = new SelectList(_context.PayoutBatches, "ID", "EmailMessage");
            return View();
        }

        // POST: Admin/Payouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipientType,Note,ID,Receiver,PayoutBatchId,Money,Currency")] Payout payout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PayoutBatchId"] = new SelectList(_context.PayoutBatches, "ID", "EmailMessage", payout.PayoutBatchId);
            return View(payout);
        }

        // GET: Admin/Payouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payout = await _context.Payouts.FindAsync(id);
            if (payout == null)
            {
                return NotFound();
            }
            ViewData["PayoutBatchId"] = new SelectList(_context.PayoutBatches, "ID", "EmailMessage", payout.PayoutBatchId);
            return View(payout);
        }

        // POST: Admin/Payouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipientType,Note,ID,Receiver,PayoutBatchId,Money,Currency")] Payout payout)
        {
            if (id != payout.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayoutExists(payout.ID))
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
            ViewData["PayoutBatchId"] = new SelectList(_context.PayoutBatches, "ID", "EmailMessage", payout.PayoutBatchId);
            return View(payout);
        }

        // GET: Admin/Payouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payout = await _context.Payouts
                .Include(p => p.PayoutBatch)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payout == null)
            {
                return NotFound();
            }

            return View(payout);
        }

        // POST: Admin/Payouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payout = await _context.Payouts.FindAsync(id);
            _context.Payouts.Remove(payout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayoutExists(int id)
        {
            return _context.Payouts.Any(e => e.ID == id);
        }
    }
}
