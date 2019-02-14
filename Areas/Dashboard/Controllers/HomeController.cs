using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Data;
using LinkShortener.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area("Dashboard")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            List<Link> links = _context.Links.Include("Clicks").Where(l => l.OwnerId == userId).ToList();

            return View(links);
        }

        public async Task<JsonResult> GetClicksLastHourAsync(string linkId)
        {
            if (linkId is null)
                return await GetAllClicksLastHourAsync();
            DateTime now = DateTime.Now;
            string nowStr = now.ToShortTimeString();
            var clicks = await _context.Clicks
                .Where(c => c.LinkId == linkId)
                .Where(c=> c.DateTime > now.AddHours(-1))
                .GroupBy(c=>c.DateTime.ToShortTimeString())
                .Select(group => 
                new {
                    Time = group.Key,
                    Count = group.Count()
                })
                .ToListAsync();
            return Json(new { clicks, now = nowStr });
        }
        private async Task<JsonResult> GetAllClicksLastHourAsync()
        {
            DateTime now = DateTime.Now;
            string userId = _userManager.GetUserId(User);
            var clicks = await _context.Clicks
                .Where(c => c.Link.OwnerId == userId)
                .Where(c => c.DateTime > DateTime.Now.AddHours(-1))
                .GroupBy(c => c.DateTime.ToShortTimeString())
                .Select(group =>
                new {
                    Time = group.Key,
                    Count = group.Count()
                })
                .ToListAsync();
            string nowStr = now.ToShortTimeString();
            return Json(new { clicks, now= nowStr});
        }
    }
}