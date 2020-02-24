using LinkShortener.Data;
using LinkShortener.Models;
using LinkShortener.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Areas.Dashboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LinkShortener.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area("Dashboard")]
    public class HomeController : Controller
    {
        private readonly AppOptions _options;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IOptionsMonitor<AppOptions> optionsAccessor, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _options = optionsAccessor.CurrentValue;
        }

        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            List<Link> links = _context.Links.Include("Clicks").Where(l => l.OwnerId == userId).ToList();

            return View(links);
        }

        public async Task<JsonResult> GetBalanceAsync()
        {
            string userId = _userManager.GetUserId(User);
            decimal money = await _context.GetBalanceAsync(userId);
            return Json(money);

        }

        public async Task<JsonResult> GetLastHourClicksAsync(string linkId)
        {
            string userId = _userManager.GetUserId(User);

            DateTime first = DateTime.Now.AddHours(-1);

            var clicks = await _context.GetLastHourClicksAsync(linkId, userId, first);

            return Json(clicks);
        }

        public async Task<JsonResult> GetLastDayClicksAsync(string linkId)
        {
            string userId = _userManager.GetUserId(User);

            DateTime first = DateTime.Now.AddDays(-1);

            var clicks = await _context.GetLastDayClicksAsync(linkId, userId, first);

            return Json(clicks);
        }

        public async Task<JsonResult> GetLastWeekClicksAsync(string linkId)
        {
            string userId = _userManager.GetUserId(User);

            DateTime first = DateTime.Now.AddDays(-7);

            var clicks = await _context.GetLastWeekClicksAsync(linkId, userId, first);

            return Json(clicks);
        }

        public async Task<JsonResult> GetLastMonthClicksAsync(string linkId)
        {
            string userId = _userManager.GetUserId(User);

            DateTime first = DateTime.Now.AddDays(-31);

            var clicks = await _context.GetLastMonthClicksAsync(linkId, userId, first);

            return Json(clicks);
        }

        public async Task<JsonResult> GetLastYearClicksAsync(string linkId)
        {
            string userId = _userManager.GetUserId(User);

            DateTime first = DateTime.Now.AddYears(-1);

            var clicks = await _context.GetLastYearClicksAsync(linkId, userId, first);

            return Json(clicks);
        }

        public IActionResult Withdraw()
        {
            PaymentMethods methods =
                new PaymentMethods("~/xml");
            ApplicationUser user = _userManager.GetUserAsync(User).Result;
            RequestVM requestVM = new RequestVM();
            if (!(user.DefaultRecipientSettings is null))
            {
                requestVM.RecipientTypeID = user.DefaultRecipientSettings.ID;
                requestVM.Receiver = user.Receiver;
            }
            ViewBag.RecipientTypeId = _context.RecipientSettings
            .Select(rt => new SelectListItem
            {
                Text = rt.RecipientType.Name,
                Value = rt.ID.ToString()
            });
            return View(requestVM);
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(RequestVM requestVM)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                decimal money = await _context.GetBalanceAsync(user.Id);
                if (money >= requestVM.Money)
                {
                    RecipientType recipient = await _context.RecipientTypes.FirstAsync(r => r.ID == requestVM.RecipientTypeID);
                    RecipientSettings recipientSettings = new RecipientSettings
                    {
                        Owner = user,
                        Receiver = requestVM.Receiver,
                        RecipientType = recipient
                    };
                    user.DefaultRecipientSettings = recipientSettings;

                    PayoutRequest payoutRequest = new PayoutRequest
                    {
                        Money = requestVM.Money,
                        RecipientSettings = recipientSettings,
                        Paid = false
                    };
                    await _context.PayoutRequests.AddAsync(payoutRequest);
                    user.RequestedMoney += requestVM.Money;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You do not have enough money");
                }
            }
            if (!(user.DefaultRecipientSettings is null))
            {
                requestVM.RecipientTypeID = user.DefaultRecipientSettings.ID;
                requestVM.Receiver = user.Receiver;
            }
            ViewBag.RecipientTypeId = _context.RecipientSettings
            .Select(rt => new SelectListItem
            {
                Text = rt.RecipientType.Name,
                Value = rt.ID.ToString()
            });
            return View(requestVM);
        }


        public async Task<decimal> Some()
        {
            string userId = _userManager.GetUserId(User);
            decimal money = await _context.GetBalanceAsync(userId);
            return money;
        }
    }
}