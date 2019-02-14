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
    public class PayoutBatchesController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public PayoutBatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/PayoutBatches
        public async Task<IActionResult> Index()
        {
            return View(await _context.PayoutBatches.ToListAsync());
        }

        // GET: Admin/PayoutBatches/Details/5
        public async Task<IActionResult> Details(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payoutBatch = await _context.PayoutBatches
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payoutBatch == null)
            {
                return NotFound();
            }

            return View(payoutBatch);
        }

        // GET: Admin/PayoutBatches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PayoutBatches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PayoutPaypalBatchId,Status,EmailMessage,EmailSubject")] PayoutBatch payoutBatch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payoutBatch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(payoutBatch);
        }

        // GET: Admin/PayoutBatches/Edit/5
        public async Task<IActionResult> Edit(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payoutBatch = await _context.PayoutBatches.FindAsync(id);
            if (payoutBatch == null)
            {
                return NotFound();
            }
            return View(payoutBatch);
        }

        // POST: Admin/PayoutBatches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DateTime id, [Bind("ID,PayoutPaypalBatchId,Status,EmailMessage,EmailSubject")] PayoutBatch payoutBatch)
        {
            if (id != payoutBatch.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payoutBatch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayoutBatchExists(payoutBatch.ID))
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
            return View(payoutBatch);
        }

        // GET: Admin/PayoutBatches/Delete/5
        public async Task<IActionResult> Delete(DateTime? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payoutBatch = await _context.PayoutBatches
                .FirstOrDefaultAsync(m => m.ID == id);
            if (payoutBatch == null)
            {
                return NotFound();
            }

            return View(payoutBatch);
        }

        // POST: Admin/PayoutBatches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DateTime id)
        {
            var payoutBatch = await _context.PayoutBatches.FindAsync(id);
            _context.PayoutBatches.Remove(payoutBatch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayoutBatchExists(DateTime id)
        {
            return _context.PayoutBatches.Any(e => e.ID == id);
        }
    }
}
