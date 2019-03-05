using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkShortener.Common;
using LinkShortener.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Link> Links { get; set; }

        public DbSet<PayoutRequest> PayoutRequests { get; set; }
        public DbSet<RecipientType> RecipientTypes { get; set; }
        public DbSet<Click> Clicks { get; set; }

        public DbSet<Payout> Payouts { get; set; }
        public DbSet<PayoutBatch> PayoutBatches { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public async Task<decimal> GetEarnedMoneyAsync(string userId, decimal moneyPerClick)
        {
            ApplicationUser user = await Users.FirstAsync(u => u.Id == userId);
            return user.EarnedMoney;
        }

        public async Task<decimal> GetAllMoneyAsync(string userId, int percentage, decimal moneyPerClick)
        {
            return await GetEarnedMoneyAsync(userId, moneyPerClick) + await GetReferralMoneyAsync(userId, percentage, moneyPerClick);
        }

        public async Task<decimal> GetReferralMoneyAsync(string userId, int percentage, decimal moneyPerClick)
        {
            decimal money = await Users.Where(u => u.ReferrerId == userId).SumAsync(u => u.EarnedMoney * percentage / 100);
            return money;
        }

        public async Task<decimal> GetRequestedMoneyAsync(string userId)
        {
            decimal money = await PayoutRequests.
            Where(p => p.OwnerId == userId).SumAsync(p => p.Money);
            return money;
        }

        public async Task<decimal> GetBalanceAsync(string userId)
        {
            ApplicationUser user = await Users.FirstAsync(u => u.Id == userId);
            return  user.EarnedMoney + user.ReferralMoney-user.RequestedMoney;
        }

        public async Task<ReferralLink> AddReferrerLinkAsync(string ownerId, string hostname)
        {
            string FullUrl = hostname + "/" + ownerId;
            Link old_link = await Links.OrderBy(l => l.Number).LastOrDefaultAsync();
            int new_num = old_link is null ? 1 : (old_link.Number + 1);
            string id = StringGenerator.getString(new_num);
            ReferralLink link = new ReferralLink
            {
                Id = id,
                Number = new_num,
                FullUrl = FullUrl,
                Clicks = new List<Click>(),
                OwnerId = ownerId
            };
            await Links.AddAsync(link);
            return link;
        }
        public async Task<Link> AddLinkAsync(string FullUrl, string ownerId)
        {
            Link old_link = await Links.OrderBy(l => l.Number).LastOrDefaultAsync();
            int new_num = old_link is null ? 1 : (old_link.Number + 1);
            string id = StringGenerator.getString(new_num);
            Link link = new Link
            {
                Id = id,
                Number = new_num,
                FullUrl = FullUrl,
                Clicks = new List<Click>(),
                OwnerId = ownerId
            };
            await Links.AddAsync(link);
            return link;
        }

        public async Task AddClickAsync(string LinkId, decimal moneyPerClick, int percentage)
        {
            Link link = await Links.FirstOrDefaultAsync(l => l.Id == LinkId);
            link.Clicks.Add(new Click
            {
                DateTime = DateTime.Now,
                LinkId = LinkId
            });

            link.Owner.EarnedMoney += moneyPerClick;
            if (link.Owner.ReferrerId != null)
            {
                link.Owner.Referrer.ReferralMoney += moneyPerClick * percentage/100;
            }

        }

        public async Task AddRandomClickAsync(string linkId, decimal moneyPerClick, int percentage)
        {
            Random r = new Random();
            Link link = await Links.FirstOrDefaultAsync(l => l.Id == linkId);
            link.Clicks.Add(new Click
            {
                DateTime = DateTime.Now.AddMinutes(-r.Next() % 60),
                LinkId = linkId
            });
            link.Owner.EarnedMoney += moneyPerClick;
            if (link.Owner.ReferrerId != null)
            {
                link.Owner.Referrer.ReferralMoney += moneyPerClick * percentage / 100;
            }
        }

        public class ClicksCount
        {
            public int Time { get; set; }
            public int Count { get; set; }

        }


        public async Task<PayoutBatch> AddPayoutBatch(List<Areas.Admin.Models.UserPayoutVM> userPayouts)
        {
            PayoutBatch payoutBatch = new PayoutBatch
            {
                EmailMessage = "Here is your monthly payout",
                EmailSubject = "PAYOUT!!!",
            };
            foreach (var userPayout in userPayouts)
            {
                payoutBatch.Payouts.Add(new Payout
                {
                    RecipientType = "EMAIL",
                    Receiver = userPayout.Email,
                    Money = userPayout.Payout,
                    Currency = "USD"
                });
            }
            await PayoutBatches.AddAsync(payoutBatch);
            return payoutBatch;
        }

        public class GraphInfo{

            public GraphInfo(int size){
                this.size = size;
                labels = new string[size];
                values = new int[size];
                for (int i = 0; i < size; i++)
                {
                    labels[i] = "";
                    values[i] = 0;
                }
            }

            public readonly int size;
            public string[] labels{get; set;}
            public int[] values{get; set;}
        }

        public async Task<GraphInfo> GetLastHourClicksAsync(string linkId, string userId, DateTime first)
        {
            bool isLinkIdNull = (linkId == null);
            List<ClicksCount> clicks = await Clicks
                .Where(c => c.Link.OwnerId == userId)
                .Where(c => isLinkIdNull ? true : c.LinkId == linkId)
                .Where(c => c.DateTime > first)
                .GroupBy(c => (int)c.DateTime.Subtract(first).TotalMinutes)
                .Select(group =>
                   new ClicksCount
                   {
                       Time = group.Key,
                       Count = group.Count()
                   })
                .ToListAsync();
            return GetGraphInfo(60, clicks, first, 1);
        }

        public async Task<GraphInfo> GetLastDayClicksAsync(string linkId, string userId, DateTime first)
        {
            bool isLinkIdNull = (linkId == null);
            List<ClicksCount> clicks = await Clicks
                .Where(c => c.Link.OwnerId == userId)
                .Where(c => isLinkIdNull ? true : c.LinkId == linkId)
                .Where(c => c.DateTime > first)
                .GroupBy(c => (int)c.DateTime.Subtract(first).TotalMinutes)
                .Select(group =>
                   new ClicksCount
                   {
                       Time = group.Key,
                       Count = group.Count()
                   })
                .ToListAsync();
            return GetGraphInfo(60*24, clicks, first, 1);
        }

        public async Task<GraphInfo> GetLastWeekClicksAsync(string linkId, string userId, DateTime first)
        {
            bool isLinkIdNull = (linkId == null);
            List<ClicksCount> clicks = await Clicks
                .Where(c => c.Link.OwnerId == userId)
                .Where(c => isLinkIdNull ? true : c.LinkId == linkId)
                .Where(c => c.DateTime > first)
                .GroupBy(c => (int)c.DateTime.Subtract(first).TotalHours)
                .Select(group =>
                   new ClicksCount
                   {
                       Time = group.Key,
                       Count = group.Count()
                   })
                .ToListAsync();
            return GetGraphInfo(24*7, clicks, first, 2);
        }

        public async Task<GraphInfo> GetLastMonthClicksAsync(string linkId, string userId, DateTime first)
        {
            bool isLinkIdNull = (linkId == null);
            List<ClicksCount> clicks = await Clicks
                .Where(c => c.Link.OwnerId == userId)
                .Where(c => isLinkIdNull ? true : c.LinkId == linkId)
                .Where(c => c.DateTime > first)
                .GroupBy(c => (int)c.DateTime.Subtract(first).TotalDays)
                .Select(group =>
                   new ClicksCount
                   {
                       Time = group.Key,
                       Count = group.Count()
                   })
                .ToListAsync();
            return GetGraphInfo(31, clicks, first, 2);
        }

        public async Task<GraphInfo> GetLastYearClicksAsync(string linkId, string userId, DateTime first)
        {
            bool isLinkIdNull = (linkId == null);
            List<ClicksCount> clicks = await Clicks
                .Where(c => c.Link.OwnerId == userId)
                .Where(c => isLinkIdNull ? true : c.LinkId == linkId)
                .Where(c => c.DateTime > first)
                .GroupBy(c => (int)c.DateTime.Subtract(first).TotalDays)
                .Select(group =>
                   new ClicksCount
                   {
                       Time = group.Key,
                       Count = group.Count()
                   })
                .ToListAsync();
            return GetGraphInfo(366, clicks, first, 2);
        }



        public GraphInfo GetGraphInfo(int size, List<ClicksCount> clicks, DateTime first, int select){
            GraphInfo info = new GraphInfo(size);
            foreach(var click in clicks){
                if(click.Time<info.size)
                {
                    if (select == 1)
                    {
                        info.labels[click.Time] = first.AddMinutes(click.Time).ToShortTimeString();
                    }
                    else if(select == 2)
                    {
                        info.labels[click.Time] = first.AddMinutes(click.Time).ToShortDateString();
                    }
                    info.values[click.Time] = click.Count;
                }
            }
            return info;
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Link>().HasIndex(l => l.Number).IsUnique();
            builder.Entity<Link>().Property(l => l.Number).HasDefaultValue(1);
            // builder.Entity<Link>().Property(l => l.Clicks).HasDefaultValue(0);
            base.OnModelCreating(builder);
        }
    }
}