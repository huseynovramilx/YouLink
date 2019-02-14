using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkShortener.Data
{
    public class DbInitializer
    {

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Clicks.Count() < 20)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Random rand = new Random();
                    double min = -(rand.NextDouble() * 60);
                    context.Clicks.Add(new Models.Click
                    {
                        DateTime = DateTime.Now.AddMinutes(min),
                        LinkId = Convert.ToString(rand.Next() % 6 + 1)

                    });
                }

            }
            context.SaveChanges();
        }
    }
}
