using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Data;
using LinkShortener.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LinkShortener.Common;

namespace LinkShortener.Controllers
{
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LinksController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Links
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Links.Include(l => l.Owner);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Links/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links
                .Include(l => l.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        // GET: Links/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullUrl")] Link link)
        {
            if (User.Identity.IsAuthenticated) {
                string id = _userManager.GetUserId(User);
                link.OwnerId = id;
            }
            if (ModelState.IsValid)
            {
                link = await _context.AddLinkAsync(link.FullUrl, link.OwnerId);
                await _context.SaveChangesAsync();
                return Ok(link.Id);
            }
            return BadRequest(ModelState);
        }

        // GET: Links/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", link.OwnerId);
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Clicks,FullUrl,OwnerId")] Link link)
        {
            if (id != link.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(link);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinkExists(link.Id))
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
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", link.OwnerId);
            return View(link);
        }

        // GET: Links/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links
                .Include(l => l.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var link = await _context.Links.FindAsync(id);
            _context.Links.Remove(link);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LinkExists(string id)
        {
            return _context.Links.Any(e => e.Id == id);
        }
    }
}
