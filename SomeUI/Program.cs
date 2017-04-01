using FullNetExample.Data;
using FullNetExample.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeUI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            _context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
            //InsertSingleSamurai();
            //InsertMultipleSamurais();
            //MoreQueies();
            //RetreiveAndUpdateSamurai();
            //RetreiveAndUpdateMultipleSamurais();
            //MultipleOperations();
            //SimpleSamuraiQuery();
            //QueryAndUpdateSamuraiDisconnected();
            //DeleteWhileTracked();
            //DeleteMany();
            //DeleteWhileNotTracked();
            //RawSqlQuery();
            //RawSqlCommand();
            //RawSqlCommandWithOutput();
            InsertNewPkFkGraph();
            InsertNewPkFkGraphMultipleChildren();
            Console.ReadLine();
        }

        private static void InsertNewPkFkGraph()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                               {
                                 new Quote {Text = "I've come to save you"}
                               }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewPkFkGraphMultipleChildren()
        {
            var samurai = new Samurai
            {
                Name = "Kyūzō",
                Quotes = new List<Quote> {
          new Quote {Text = "Watch out for my sharp sword!"},
          new Quote {Text="I told you to watch out for the sharp sword! Oh well!" }
        }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewOneToOneGraph()
        {
            var samurai = new Samurai { Name = "Shichirōji " };
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julie" };
            _context.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddChildToExistingObjectWhileTracked()
        {
            var samurai = _context.Samurais.First();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void AddOneToOneToExistingObjectWhileTracked()
        {
            var samurai = _context.Samurais
              .FirstOrDefault(s => s.SecretIdentity == null);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            _context.SaveChanges();
        }

        private static void AddBattles()
        {
            _context.Battles.AddRange(
              new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },
              new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },
              new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
              );
            _context.SaveChanges();
        }

        private static void AddManyToManyWithFks()
        {
            _context = new SamuraiContext();
            var sb = new SamuraiBattle { SamuraiId = 1, BattleId = 1 };
            _context.SamuraiBattles.Add(sb);
            _context.SaveChanges();
        }

        private static void AddManyToManyWithObjects()
        {
            _context = new SamuraiContext();
            var samurai = _context.Samurais.FirstOrDefault();
            var battle = _context.Battles.FirstOrDefault();
            _context.SamuraiBattles.Add(
             new SamuraiBattle { Samurai = samurai, Battle = battle });
            _context.SaveChanges();
        }

        private static void DeleteWhileNotTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Heihachi Hayashida");
            using (var contextNewAppInstance = new SamuraiContext())
            {
                contextNewAppInstance.Samurais.Remove(samurai);
                //contextNewAppInstance.Entry(samurai).State=EntityState.Deleted;
                contextNewAppInstance.SaveChanges();
            }

        }

        private static void RawSqlQuery()
        {
            //var samurais= _context.Samurais.FromSql("Select * from Samurais")
            //              .OrderByDescending(s => s.Name)
            //              .Where(s=>s.Name.Contains("San")).ToList();
            var namePart = "San";

            // the order by clause and where clause will be performed on the db server side
            var samurais = _context.Samurais
              .FromSql("EXEC FilterSamuraiByNamePart {0}", namePart)
              .OrderByDescending(s => s.Name).ToList();

            samurais.ForEach(s => Console.WriteLine(s.Name));
            Console.WriteLine();
        }

        private static void QueryWithNonSql()
        {
            var samurais = _context.Samurais
              .Select(s => new { newName = ReverseString(s.Name) })
              .ToList();
            samurais.ForEach(s => Console.WriteLine(s.newName));
            Console.WriteLine();
        }

        private static string ReverseString(string value)
        {
            var stringChar = value.AsEnumerable();
            return string.Concat(stringChar.Reverse());
        }

        private static void RawSqlCommand()
        {
            var affected = _context.Database.ExecuteSqlCommand(
              "update samurais set Name=REPLACE(Name,'San','Nan')");
            Console.WriteLine($"Affected rows {affected}");
        }

        private static void RawSqlCommandWithOutput()
        {
            var procResult = new SqlParameter
            {
                ParameterName = "@procResult",
                SqlDbType = SqlDbType.VarChar,
                Direction = ParameterDirection.Output,
                Size = 50
            };
            _context.Database.ExecuteSqlCommand(
              "exec FindLongestName @procResult OUT", procResult);
            Console.WriteLine($"Longest name: {procResult.Value}");
        }

        private static void DeleteMany()
        {
            var samurais = _context.Samurais.ToList();
            _context.RemoveRange(samurais);
            _context.SaveChanges();
        }

        private static void DeleteWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();

            // entityframework could only delete record that is already tracked by the context
            _context.Samurais.Remove(samurai);

            // alternatives:
            //_context.Remove(samurai);
            //_context.Entry(samurai).State = EntityState.Deleted;
            //_context.Samurais.Remove(_context.Samurais.Find(1));

            _context.SaveChanges();
        }

        private static void QueryAndUpdateSamuraiDisconnected()
        {
            // simulate object deserialized from HTTP PUT request
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name = $"Updated name: {samurai.Name}";

            using (var context = new SamuraiContext())
            {
                // the sql update query will include all object properties
                context.Update(samurai);
                context.SaveChanges();
            }
        }

        private static void MultipleOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name = $"Updated name: {samurai.Name}";
            _context.Samurais.Add(new Samurai
            {
                Name = "New samurai"
            });
            _context.SaveChanges();
        }

        private static void RetreiveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.ToList();
            foreach (var samurai in samurais)
            {
                samurai.Name = $"Updated name: {samurai.Name}";
            }
            _context.SaveChanges();
        }

        private static void RetreiveAndUpdateSamurai()
        {
            // the sql update query will set the specific property being updated
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name = $"Updated name: {samurai.Name}";
            _context.SaveChanges();
        }

        private static void MoreQueies()
        {
            var samurais = _context.Samurais.Where(s => s.Name == "Tony").ToList();

            // compared with the above, the query below will be parameterized because the name parameter comes from a variable
            // while the query above has a hard-coded name in code
            //var name = "Tony";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();

        }

        private static void SimpleSamuraiQuery()
        {
            using (var context = new SamuraiContext())
            {
                var allSamurais = context.Samurais.ToList();
                foreach (var samurai in allSamurais)
                {
                    Console.WriteLine($"name: {samurai.Name}");
                }

                // fetching the first item, doing some work on it, then the next item, doing some work on it...
                // the connection is kept open until the last item is fetched
                //foreach (var samurai in context.Samurais)
                //{
                //    // work... work... work...
                //}
            }
        }

        private static void InsertMultipleSamurais()
        {
            var samurais = new List<Samurai> {
                new Samurai
                {
                    Name = "Multiple Tony"
                },
                new Samurai
                {
                    Name = "Multiple Tony 2"
                },
            };
            using (var context = new SamuraiContext())
            {
                context.Samurais.AddRange(samurais);
                context.SaveChanges();
            }
        }

        private static void InsertSingleSamurai()
        {
            var samurai = new Samurai
            {
                Name = "Tony"
            };
            using (var context = new SamuraiContext())
            {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }
    }
}
