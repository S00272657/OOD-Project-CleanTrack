using CleanTrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTrack
{
    public class Job : IComparable<Job>
    {
        //properties
        public int JobId { get; set; }
        public string JobType { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }        // Pending/Completed/Cancelled
        public string Address { get; set; }
        public string JobDescription { get; set; }
        public string TimeTaken { get; set; }
        public string AssignedEmployees => Employees != null && Employees.Any()
            ? string.Join(", ", Employees.Select(e => e.FullName))
            : "Unassigned";


        //foreign key to client & one client per job
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        // list of employee(s) (many-to-one)
        public virtual List<Employee> Employees { get; set; }


        // constructors
        public Job()
        {
            Status = "Booked";
            Date = DateTime.Now;
            Employees = new List<Employee>();
        }

        public Job(string jobType, decimal price, DateTime date, string address, int clientId)
        {
            JobType = jobType;
            Price = price;
            Date = date;
            Address = address;
            ClientId = clientId;
            Status = "Booked";
            Employees = new List<Employee>();
        }

        // IComparable<Job> — sorts jobs chronologically by date
        public int CompareTo(Job other)
        {
            if (other == null) return 1;
            return this.Date.CompareTo(other.Date);
        }
    }
}


