using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTrack
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } //cleaner/supervisor/manager
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        //jobs theyve worked on (mant-to-one)
        public virtual List<Job> Jobs { get; set; }
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        // constructors
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
