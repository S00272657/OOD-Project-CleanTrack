using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CleanTrack
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("MyData") { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
