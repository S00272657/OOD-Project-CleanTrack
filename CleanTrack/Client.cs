using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTrack
{
    public class Client : Person
    {
        public int ClientId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Eircode { get; set; }
        public bool IsActive { get; set; }
        public virtual List<Job> Jobs { get; set; }

        public Client()
        {
            Jobs = new List<Job>();
            IsActive = false;
        }

        public Client(string firstName, string lastName, string phoneNumber, string email, string address, string city, string eircode)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
            City = city;
            Eircode = eircode;
            Jobs = new List<Job>();
            IsActive = true;
        }
    }
}
