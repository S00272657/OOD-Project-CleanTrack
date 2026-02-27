using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CleanTrack;

namespace DataManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDbContext db = new AppDbContext();

            using (db)
            {
                // Clients
                Client c1 = new Client("John", "Murphy", "0851234567", "john.murphy@gmail.com", "12 Oak Street", "Dublin", "D01 AB12");
                Client c2 = new Client("Sarah", "O'Brien", "0879876543", "sarah.obrien@gmail.com", "45 Maple Avenue", "Cork", "T12 CD34");
                Client c3 = new Client("James", "Kelly", "0861112233", "james.kelly@gmail.com", "8 Pine Road", "Galway", "H91 EF56");

                // Employees
                Employee e1 = new Employee("Liam", "Walsh", "0851111111", "liam.walsh@cleantrack.com", "Cleaner");
                Employee e2 = new Employee("Emma", "Ryan", "0852222222", "emma.ryan@cleantrack.com", "Cleaner");
                Employee e3 = new Employee("Conor", "Byrne", "0853333333", "conor.byrne@cleantrack.com", "Supervisor");

                db.Clients.Add(c1);
                db.Clients.Add(c2);
                db.Clients.Add(c3);
                Console.WriteLine("added clients to database");

                db.Employees.Add(e1);
                db.Employees.Add(e2);
                db.Employees.Add(e3);
                Console.WriteLine("added employees to database");

                db.SaveChanges();

                // Jobs
                Job j1 = new Job("House", 120.00m, DateTime.Now, "12 Oak Street", c1.ClientId);
                Job j2 = new Job("Carpet", 85.00m, DateTime.Now.AddDays(-3), "45 Maple Avenue", c2.ClientId);
                Job j3 = new Job("Window", 60.00m, DateTime.Now.AddDays(-7), "8 Pine Road", c3.ClientId);

                db.Jobs.Add(j1);
                db.Jobs.Add(j2);
                db.Jobs.Add(j3);
                Console.WriteLine("added jobs to database");

                j1.Employees.Add(e1);
                j2.Employees.Add(e2);
                j3.Employees.Add(e3);

                db.SaveChanges();
            }
        }
    }
}
