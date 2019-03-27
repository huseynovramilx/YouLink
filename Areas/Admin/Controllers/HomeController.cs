using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Areas.Admin.Models;
using LinkShortener.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkShortener.Models;
using LinkShortener.Models;
using Microsoft.Extensions.Options;

namespace LinkShortener.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : AdminBaseController
    {

        private readonly AppOptions _options;
        private readonly ApplicationDbContext _context;
        private readonly PayPalHttpClientFactory _clientFactory;
        public HomeController(IOptionsMonitor<AppOptions> optionsAccessor, ApplicationDbContext context, PayPalHttpClientFactory clientFactory)
        {
            _options = optionsAccessor.CurrentValue;
            _context = context;
            _clientFactory = clientFactory;
        }

        private List<UserPayoutVM> GetCurrentPayouts()
        {
            PayoutBatch lastPayoutBatch = _context.PayoutBatches.OrderBy(p => p.ID).LastOrDefault();
            List<UserPayoutVM> userPayouts = _context.Links
            .Include(l => l.Owner)
            .Include(l => l.Clicks)
            .GroupBy(l => l.OwnerId)
            .Where(group => group.First().OwnerId != null)
            .Select(group => new UserPayoutVM
            {
                Email = group.First().Owner.Email,
                Payout = _options.MoneyPerClick * group.Sum(l => l.Clicks.Where(
                      c => (c.DateTime <= DateTime.Now.AddDays(-1) && (lastPayoutBatch == null || c.DateTime >= lastPayoutBatch.ID)))
                  .Count())
            }).Where(u => u.Payout != 0).ToList();
            if (userPayouts == null)
                return new List<UserPayoutVM>();

            return userPayouts;

        }


        public IActionResult Index()
        {
            return View(GetCurrentPayouts());
        }


        [HttpPost]
        public async Task<IActionResult> SubmitPayouts()
        {
            PayoutBatch payoutBatch = await _context.AddPayoutBatch(GetCurrentPayouts());
            if (payoutBatch.PayoutPaypalBatchId == null)
            {
                PayPalHttpClient client = _clientFactory.GetClient();
                try
                {
                    await client.AuthorizeAsync();
                    await client.CreatePayouts(payoutBatch);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }
            }

            return View(viewName: "Index",model: new List<UserPayoutVM>());
        }
    }
}