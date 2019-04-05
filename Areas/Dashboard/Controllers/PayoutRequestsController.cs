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
            var applicationDbContext = _context.PayoutRequests;
            return View(await applicationDbContext.ToListAsync());
        }
    }
}
