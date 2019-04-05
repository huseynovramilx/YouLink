using LinkShortener.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Data
{
    public class DbInitializer
    {

        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOptionsMonitor<AppOptions> optionsAccessor)
        {
            context.Database.EnsureCreated();
            //UpdateUsers(ref context, optionsAccessor.CurrentValue.MoneyPerClick, optionsAccessor.CurrentValue.ReferralPercentage);
            //context.SaveChanges();
            List<string> userIds = context.Users.Select(u => u.Id).ToList();
            //AddUsers(ref context, userManager, userIds);

            //List<string> linkIds = context.Links.Select(l => l.Id).ToList();
            //AddLinks(ref context, userIds, linkIds);

            //AddClicks(ref context, linkIds, optionsAccessor.CurrentValue.ReferralPercentage);

            AddPayoutRequests(ref context, userIds);
        }


        private static void UpdateUsers(ref ApplicationDbContext c, decimal moneyPerClick, int percentage)
        {
            foreach (ApplicationUser user in c.Users)
            {
                user.ReferralMoney = c.GetReferralMoneyAsync(user.Id, percentage, moneyPerClick).Result;
            }
        }

        public static void AddPayoutRequests(ref ApplicationDbContext c, List<string> userIds)
        {
            foreach (string userId in userIds)
            {
                for (int i = 0; i < 10; i++)
                {
                    decimal money = c.GetBalanceAsync(userId).Result;

                    if (money <= 50)
                        break;
                    AddPayoutRequest(ref c, money / 5, userId, i % 3 == 1);
                    c.SaveChanges();
                }
            }
            
        }

        public static void AddPayoutRequest(ref ApplicationDbContext context, decimal money, string ownerId, bool paid)
        {
         
            RecipientType recipient = context.RecipientTypes.First(r => r.ID == 1);
            ApplicationUser user = context.Users.First(u => u.Id == ownerId);
            RecipientSettings recipientSettings = new RecipientSettings
            {
                Owner = user,
                Receiver = ownerId,
                RecipientType = recipient
            };
            user.DefaultRecipientSettings = recipientSettings;

            PayoutRequest payoutRequest = new PayoutRequest
            {
                Money = money,
                RecipientSettings = recipientSettings,
                Paid = false
            };
            user.RequestedMoney += money;
        }

        public static void AddClicks(ref ApplicationDbContext context, List<string> linkIds, int percentage)
        {
            Random r = new Random();
            foreach (string linkId in linkIds)
            {
                for (int i = 0; i < r.Next() % 200; i++)
                {
                    context.AddRandomClickAsync(linkId, percentage).Wait();

                }
            }

            context.SaveChanges();

        }

        public static void AddUsers(ref ApplicationDbContext c, UserManager<ApplicationUser> u, List<string> users)
        {
            Currency cur1 = c.Currencies.First();
            Currency cur2 = c.Currencies.Last();

            for (int i = 0; i < 10; i++)
            {
                string id1 = AddUser(ref c, ref u, "huseynovramil" + i.ToString() + "@gmail.com", "Ramil123*", cur1);
                string id2 = AddUser(ref c, ref u, "aliyevvaqif" + i.ToString() + "@gmail.com", "Vaqif123*", cur2, id1);
                string id3 = AddUser(ref c, ref u, "axundovzakir" + i.ToString() + "@gmail.com", "Zakir123*", cur1, id2);
                string id4 = AddUser(ref c, ref u, "agakisiyevorxan" + i.ToString() + "@gmail.com", "Orxan123*", cur2, id3);
                string id5 = AddUser(ref c, ref u, "nezerovyusif" + i.ToString() + "@gmail.com", "Yusif123*", cur1, id4);
                string id6 = AddUser(ref c, ref u, "sahidovoruc" + i.ToString() + "@gmail.com", "Oruc123*", cur2, id4);
                string id7 = AddUser(ref c, ref u, "xelilovkenan" + i.ToString() + "@gmail.com", "Kenan123*", cur2);
                string id8 = AddUser(ref c, ref u, "selimovmurad" + i.ToString() + "@gmail.com", "Murad123*", cur1, id3);
                string id9 = AddUser(ref c, ref u, "hesimovferid" + i.ToString() + "@gmail.com", "Ferid123*", cur2, id2);
                string id10 = AddUser(ref c, ref u, "kerimovcavid" + i.ToString() + "@gmail.com", "Cavid123*", cur1, id1);
                users.Add(id1);
                users.Add(id2);
                users.Add(id3);
                users.Add(id4);
                users.Add(id5);
                users.Add(id6);
                users.Add(id7);
                users.Add(id8);
                users.Add(id9);
                users.Add(id10);

            }
        }

        public static string AddLink(ref ApplicationDbContext context, string link, string ownerId)
        {
            Link l = new Link();
            context.AddLinkAsync(link, ownerId).Wait();
            context.SaveChanges();
            return l.Id;
        }


        public static void AddLinks(ref ApplicationDbContext c, List<string> userIds, List<string> linkIds)
        {
            if (c.Links.Count() < 500)
            {
                foreach (string userId in userIds)
                {
                    linkIds.Add(AddLink(ref c, "https://zor.com", userId));
                    linkIds.Add(AddLink(ref c, "https://google.com", userId));
                    linkIds.Add(AddLink(ref c, "https://youtube.com", userId));
                    linkIds.Add(AddLink(ref c, "https://facebook.com", userId));
                    linkIds.Add(AddLink(ref c, "https://instagram.com", userId));
                    linkIds.Add(AddLink(ref c, "https://books.com", userId));
                    linkIds.Add(AddLink(ref c, "https://cool.com", userId));
                    linkIds.Add(AddLink(ref c, "https://yahoo.com", userId));
                    linkIds.Add(AddLink(ref c, "https://gmail.com", userId));
                    linkIds.Add(AddLink(ref c, "https://links.com", userId));

                }
            }
            else
            {
                linkIds.AddRange(c.Links.Select(l => l.Id).ToList());
            }
        }

        public static string AddUser(ref ApplicationDbContext context, ref UserManager<ApplicationUser> userManager, string email, string password, Currency cur, string referrerId = null)
        {

            var user = new ApplicationUser
            {
                Email = email,
                UserName = email,
                Currency = new Currency {
                    ID = 1
                },
                ReferrerId = referrerId
            };
            userManager.CreateAsync(user, password).Wait();

            context.AddReferrerLinkAsync(user.Id, "https://localhost:5001/Identity/Account/Register").Wait();
            context.SaveChanges();
            return user.Id;
        }
    }
}
