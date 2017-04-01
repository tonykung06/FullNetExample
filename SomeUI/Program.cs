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
            RawSqlCommandWithOutput();
            Console.ReadLine();
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
