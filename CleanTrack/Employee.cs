using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTrack
{
    public class Employee : Person
    {
        public int EmployeeId { get; set; }
        public string Role { get; set; } // cleaner/supervisor/manager
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public string ActiveStatus => IsActive ? "Active" : "Inactive";

        public virtual List<Job> Jobs { get; set; }

        public Employee()
        {
            Jobs = new List<Job>();
            IsActive = true;
            StartDate = DateTime.Now;
        }

        public Employee(string firstName, string lastName, string phoneNumber, string email, string role)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Role = role;
            Jobs = new List<Job>();
            IsActive = true;
            StartDate = DateTime.Now;
        }
    }
}
