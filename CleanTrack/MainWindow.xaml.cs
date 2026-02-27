using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanTrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           dgJobs.ItemsSource = jobs;
        }

        //array of jobs for ui testing
        List<Job> jobs = new List<Job>
        {
            new Job { JobId = 1, JobType = "House", Price = 120.00m, Date = DateTime.Now, Address = "12 Oak Street, Dublin", Status = "Completed", JobDescription = "Full house clean", ClientId = 1 },
            new Job { JobId = 2, JobType = "Carpet", Price = 85.00m, Date = DateTime.Now.AddDays(-2), Address = "45 Maple Avenue, Cork", Status = "Pending", JobDescription = "Living room carpet clean", ClientId = 1 },
            new Job { JobId = 3, JobType = "Window", Price = 60.00m, Date = DateTime.Now.AddDays(-4), Address = "8 Pine Road, Galway", Status = "Completed", JobDescription = "External windows", ClientId = 1 },
            new Job { JobId = 4, JobType = "House", Price = 150.00m, Date = DateTime.Now.AddDays(-6), Address = "3 Elm Drive, Limerick", Status = "Cancelled", JobDescription = "Deep clean", ClientId = 1 },
            new Job { JobId = 5, JobType = "Carpet", Price = 95.00m, Date = DateTime.Now.AddDays(1), Address = "22 Birch Lane, Waterford", Status = "Pending", JobDescription = "Stairs and landing", ClientId = 1 }
        };
        //array of clients for ui testing
        List<Client> clients = new List<Client>
        {
            new Client { ClientId = 1, FirstName = "John", LastName = "Murphy", PhoneNumber = "0851234567", Email = "john.murphy@gmail.com", Address = "12 Oak Street", City = "Dublin", Eircode = "D01 AB12", IsActive = true },
            new Client { ClientId = 2, FirstName = "Sarah", LastName = "O'Brien", PhoneNumber = "0879876543", Email = "sarah.obrien@gmail.com", Address = "45 Maple Avenue", City = "Cork", Eircode = "T12 CD34", IsActive = true },
            new Client { ClientId = 3, FirstName = "James", LastName = "Kelly", PhoneNumber = "0861112233", Email = "james.kelly@gmail.com", Address = "8 Pine Road", City = "Galway", Eircode = "H91 EF56", IsActive = true},
            new Client { ClientId = 4, FirstName = "Emma", LastName = "Doyle", PhoneNumber = "0832223344", Email = "emma.doyle@gmail.com", Address = "3 Elm Drive", City = "Limerick", Eircode = "V94 GH78", IsActive = false},
            new Client { ClientId = 5, FirstName = "Conor", LastName = "Walsh", PhoneNumber = "0853334455", Email = "conor.walsh@gmail.com", Address = "22 Birch Lane", City = "Waterford", Eircode = "X91 IJ90", IsActive = true}
        };


    }
}
