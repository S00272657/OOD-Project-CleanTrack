using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
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
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            dgJobs.ItemsSource = jobs;
        }

        #region JobsTab
        //JOB OBJECT: JobId, JobType, Price, Date, Status, Address, JobDescription, TimeTaken, ClientId, Client, Employees
        //I want to display: Date, JobType, Price, Client.Name, Status, Address, JobDescription, timetaken, employee

        //array of jobs for ui testing
        ObservableCollection<Job> jobs = new ObservableCollection<Job>
        {
            new Job { JobId = 1, JobType = "House Cleaning", Price = 120.00m, Date = DateTime.Now, Status = "Completed", Address = "12 Oak Street, Dublin", JobDescription = "Full house clean", TimeTaken = "2 hours", Client = new Client { FirstName = "John", LastName = "Murphy" }},
            new Job { JobId = 2, JobType = "Carpet Cleaning", Price = 85.00m, Date = DateTime.Now.AddDays(-2), Status = "Pending", Address = "45 Maple Avenue, Cork", JobDescription = "Living room carpet clean", TimeTaken = "1.5 hours", Client = new Client { FirstName = "Sarah", LastName = "O'Brien" }},
            new Job { JobId = 3, JobType = "Window Cleaning", Price = 60.00m, Date = DateTime.Now.AddDays(-4), Status = "Completed", Address = "8 Pine Road, Galway", JobDescription = "External windows front and back", TimeTaken = "1 hour", Client = new Client { FirstName = "James", LastName = "Kelly" }},
            new Job { JobId = 4, JobType = "After-Builder Cleaning", Price = 150.00m, Date = DateTime.Now.AddDays(-6), Status = "Cancelled", Address = "3 Elm Drive, Limerick", JobDescription = "Post renovation deep clean", TimeTaken = "3 hours", Client = new Client { FirstName = "Emma", LastName = "Doyle" }},
            new Job { JobId = 5, JobType = "Upholstery Cleaning", Price = 95.00m, Date = DateTime.Now.AddDays(1), Status = "Pending", Address = "22 Birch Lane, Waterford", JobDescription = "Sofa and armchairs", TimeTaken = "1.5 hours", Client = new Client { FirstName = "Conor", LastName = "Walsh" }}
        };

        private void FilterResults()
        {
            var filtered = jobs.AsQueryable();

            //filter by search 
            string search = tbxSearchJobs.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(j =>
                    j.Address.ToLower().Contains(search) ||
                    j.Client.FullName.ToLower().Contains(search) ||
                    j.JobDescription.ToLower().Contains(search));
            }

            //filter by status
            if (cbxStatusFilter.SelectedItem is ComboBoxItem statusItem && statusItem.Content.ToString() != "All")
            {
                filtered = filtered.Where(j => j.Status == statusItem.Content.ToString());
            }

            //filter by job type
            if (cbxJobTypeFilter.SelectedItem is ComboBoxItem typeItem && typeItem.Content.ToString() != "All")
            {
                filtered = filtered.Where(j => j.JobType == typeItem.Content.ToString());
            }
            //filter by employee
            if (cbxEmployeeFilter.SelectedItem is ComboBoxItem employeeItem && employeeItem.Content.ToString() != "All")
            {
                filtered = filtered.Where(j => j.Employees.Any(e => e.FullName == employeeItem.Content.ToString()));
            }

            //filter by date range
            if (dpFromDateFilter.SelectedDate.HasValue)
            {
                filtered = filtered.Where(j => j.Date >= dpFromDateFilter.SelectedDate.Value);
            }
            if (dpToDateFilter.SelectedDate.HasValue)
            {
                filtered = filtered.Where(j => j.Date <= dpToDateFilter.SelectedDate.Value);
            }
            dgJobs.ItemsSource = null;
            dgJobs.ItemsSource = filtered.ToList();
        }

        #region eventListeners
        //EVENT LISTENERS
        private void tbxSearchJobs_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterResults();
        }

        private void cbxStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterResults();
        }

        private void cbxJobTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterResults();
        }

        private void cbxEmployeeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterResults();
        }

        private void dpFromDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterResults();
        }

        private void dpToDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterResults();
        }
        #endregion

        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            
        }
        #endregion







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
