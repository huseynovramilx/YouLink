using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinkShortener.Common;
using LinkShortener.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Data {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public DbSet<Link> Links { get; set; }

        public DbSet<Click> Clicks { get; set; }

        public DbSet<Payout> Payouts { get; set; }
        public DbSet<PayoutBatch> PayoutBatches { get; set; }

        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) { }

        /*  public decimal GetMoney(string userId){
             decimal money = 0;
             foreach (Link link in Links.Include(l => l.Clicks).Where(l=>l.OwnerId==userId))
             {
                 money += link.Clicks.Count;
             }
             return money;
         }*/
        public async Task<Link> AddLinkAsync (string FullUrl, string ownerId) {
            Link old_link = await Links.OrderBy (l => l.Number).LastOrDefaultAsync ();
            int new_num = old_link is null? 1: (old_link.Number + 1);
            string id = StringGenerator.getString (new_num);
            Link link = new Link {
                Id = id,
                Number = new_num,
                FullUrl = FullUrl,
                Clicks = new List<Click> (),
                OwnerId = ownerId
            };
            await Links.AddAsync (link);
            return link;
        }

        public async Task AddClickAsync (string LinkId) {
            Link link = await Links.FirstOrDefaultAsync (l => l.Id == LinkId);
            link.Clicks.Add (new Click {
                DateTime = DateTime.Now,
                    LinkId = LinkId
            });
        }

        public async Task<PayoutBatch> AddPayoutBatch(List<Areas.Admin.Models.UserPayoutVM> userPayouts) {
            PayoutBatch payoutBatch = new PayoutBatch{
                EmailMessage = "Here is your monthly payout",
                EmailSubject = "PAYOUT!!!",
            };
            foreach(var userPayout in userPayouts){
                payoutBatch.Payouts.Add(new Payout{
                    RecipientType = "EMAIL",
                    Receiver = userPayout.Email,
                    Money = userPayout.Payout,
                    Currency = "USD"
                });
            }
            await PayoutBatches.AddAsync(payoutBatch);
            return payoutBatch;
        }

        protected override void OnModelCreating (ModelBuilder builder) {
            builder.Entity<Link> ().HasIndex (l => l.Number).IsUnique ();
            builder.Entity<Link> ().Property (l => l.Number).HasDefaultValue (1);
            // builder.Entity<Link>().Property(l => l.Clicks).HasDefaultValue(0);
            base.OnModelCreating (builder);
        }
    }
}