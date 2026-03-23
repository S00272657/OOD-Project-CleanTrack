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
            LoadData();
        }

        #region JobsTab
        //JOB OBJECT: JobId, JobType, Price, Date, Status, Address, JobDescription, TimeTaken, ClientId, Client, Employees
        //I want to display: Date, JobType, Price, Client.Name, Status, Address, JobDescription, timetaken, employee

        private void FilterJobResults()
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

        //event listeners for filters
        #region filterEventListeners
        //EVENT LISTENERS
        private void tbxSearchJobs_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterJobResults();
        }

        private void cbxStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterJobResults();
        }

        private void cbxJobTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterJobResults();
        }

        private void cbxEmployeeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterJobResults();
        }

        private void dpFromDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterJobResults();
        }

        private void dpToDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterJobResults();
        }
        #endregion

        //action buttons: add, edit, delete, view details, reset filters
        #region ActionButtons
        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            AddJobWindow addJobWindow = new AddJobWindow(clients.ToList(), employees.ToList());
            addJobWindow.Owner = this;

            if (addJobWindow.ShowDialog() == true)
            {
                jobs.Add(addJobWindow.NewJob);
                FilterJobResults();
            }
        }
        private void btnDeleteJob_Click(object sender, RoutedEventArgs e)
        {
            // check something is selected
            if (dgJobs.SelectedItem == null)
            {
                MessageBox.Show("Please select a job to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Job selectedJob = dgJobs.SelectedItem as Job;

            // confirmation dialog
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete this job?\n\n{selectedJob.JobType} at {selectedJob.Address}",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                jobs.Remove(selectedJob);
                FilterJobResults();

                // when db is connected:
                // db.Jobs.Remove(selectedJob);
                // db.SaveChanges();
            }
        }
        private void btnEditJob_Click(object sender, RoutedEventArgs e)
        {
            if (dgJobs.SelectedItem == null)
            {
                MessageBox.Show("Please select a job to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Job selectedJob = dgJobs.SelectedItem as Job;

            EditJobWindow editJobWindow = new EditJobWindow(selectedJob, clients.ToList(), employees.ToList());
            editJobWindow.Owner = this;

            if (editJobWindow.ShowDialog() == true)
            {
                // job object is updated so just refresh display
                FilterJobResults();

                // when db is connected:
                // db.SaveChanges();
            }
        }
        private void btnViewJobDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgJobs.SelectedItem == null)
            {
                MessageBox.Show("Please select a job to view.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Job selectedJob = dgJobs.SelectedItem as Job;
            JobDetailsWindow detailsWindow = new JobDetailsWindow(selectedJob);
            detailsWindow.Owner = this;
            detailsWindow.ShowDialog();
        }
        private void btnResetJobFilters_Click(object sender, RoutedEventArgs e)
        {
            tbxSearchJobs.Text = string.Empty;
            cbxStatusFilter.SelectedIndex = 0;
            cbxJobTypeFilter.SelectedIndex = 0;
            cbxEmployeeFilter.SelectedIndex = 0;
            dpFromDateFilter.SelectedDate = null;
            dpToDateFilter.SelectedDate = null;

            dgJobs.ItemsSource = jobs;
        }
        #endregion
        #endregion

        #region ClientsTab
        //filtering method
        //event listeners for filters
        //action buttons: add, edit, delete, view details, reset filters
        #endregion

        #region EmployeesTab
        private void FilterEmployeeResults()
        {
            var filtered = employees.AsQueryable();

            // filter by search
            string search = tbxSearchEmployees.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(e =>
                    e.FullName.ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search) ||
                    e.PhoneNumber.Contains(search));
            }

            // filter by role
            if (cbxRoleFilter.SelectedItem is ComboBoxItem roleItem && roleItem.Content.ToString() != "All")
            {
                filtered = filtered.Where(e => e.Role == roleItem.Content.ToString());
            }

            // filter by active status
            if (cbxActiveFilter.SelectedItem is ComboBoxItem activeItem && activeItem.Content.ToString() != "All")
            {
                bool isActive = activeItem.Content.ToString() == "Active";
                filtered = filtered.Where(e => e.IsActive == isActive);
            }

            dgEmployees.ItemsSource = null;
            dgEmployees.ItemsSource = filtered.ToList();
        }

        #region EmployeeFilterEventListeners
        private void tbxSearchEmployees_SelectionChanged(object sender, RoutedEventArgs e)
        {
            FilterEmployeeResults();
        }

        private void cbxRoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEmployeeResults();
        }

        private void cbxActiveFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEmployeeResults();
        }
        #endregion
        #region EmployeeActionButtons
        private void btnResetEmployeeFilters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Employee selectedEmployee = dgEmployees.SelectedItem as Employee;

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete {selectedEmployee.FullName}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                employees.Remove(selectedEmployee);
                FilterEmployeeResults();

                // when db connected:
                // _db.Employees.Remove(selectedEmployee);
                // _db.SaveChanges();
            }
        }

        private void btnViewEmployeeDetails_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #endregion



        private ObservableCollection<Client> clients;
        private ObservableCollection<Job> jobs;
        private ObservableCollection<Employee> employees;
        //DUMMY DATA FOR TESTING
        private void LoadData()
        {
            // clients first
            clients = new ObservableCollection<Client>
            {
                new Client { ClientId = 1, FirstName = "John", LastName = "Murphy", PhoneNumber = "0851234567", Email = "john.murphy@gmail.com", Address = "12 Oak Street", City = "Dublin", Eircode = "D01 AB12", IsActive = true },
                new Client { ClientId = 2, FirstName = "Sarah", LastName = "O'Brien", PhoneNumber = "0879876543", Email = "sarah.obrien@gmail.com", Address = "45 Maple Avenue", City = "Cork", Eircode = "T12 CD34", IsActive = true },
                new Client { ClientId = 3, FirstName = "James", LastName = "Kelly", PhoneNumber = "0861112233", Email = "james.kelly@gmail.com", Address = "8 Pine Road", City = "Galway", Eircode = "H91 EF56", IsActive = true },
                new Client { ClientId = 4, FirstName = "Emma", LastName = "Doyle", PhoneNumber = "0832223344", Email = "emma.doyle@gmail.com", Address = "3 Elm Drive", City = "Limerick", Eircode = "V94 GH78", IsActive = false },
                new Client { ClientId = 5, FirstName = "Conor", LastName = "Walsh", PhoneNumber = "0853334455", Email = "conor.walsh@gmail.com", Address = "22 Birch Lane", City = "Waterford", Eircode = "X91 IJ90", IsActive = true }
            };

            // employees second
            employees = new ObservableCollection<Employee>
            {
                new Employee { EmployeeId = 1, FirstName = "Liam", LastName = "Walsh", PhoneNumber = "0851111111", Email = "liam.walsh@cleantrack.com", Role = "Cleaner", IsActive = true, StartDate = new DateTime(2023, 1, 15) },
                new Employee { EmployeeId = 2, FirstName = "Emma", LastName = "Ryan", PhoneNumber = "0852222222", Email = "emma.ryan@cleantrack.com", Role = "Cleaner", IsActive = true, StartDate = new DateTime(2023, 3, 20) },
                new Employee { EmployeeId = 3, FirstName = "Conor", LastName = "Byrne", PhoneNumber = "0853333333", Email = "conor.byrne@cleantrack.com", Role = "Supervisor", IsActive = true, StartDate = new DateTime(2022, 6, 1) }
            };

            // jobs last - can now reference clients by ID
            jobs = new ObservableCollection<Job>
            {
                new Job { JobId = 1, JobType = "House Cleaning", Price = 120.00m, Date = DateTime.Now, Status = "Completed", Address = "12 Oak Street, Dublin", JobDescription = "Full house clean", TimeTaken = "2 hours", ClientId = 1, Client = clients.FirstOrDefault(c => c.ClientId == 1)},
                new Job { JobId = 2, JobType = "Carpet Cleaning", Price = 85.00m, Date = DateTime.Now.AddDays(-2), Status = "Scheduled", Address = "45 Maple Avenue, Cork", JobDescription = "Living room carpet clean", TimeTaken = "1.5 hours", ClientId = 2, Client = clients.FirstOrDefault(c => c.ClientId == 2)},
                new Job { JobId = 3, JobType = "Window Cleaning", Price = 60.00m, Date = DateTime.Now.AddDays(-4), Status = "Completed", Address = "8 Pine Road, Galway", JobDescription = "External windows front and back", TimeTaken = "1 hour", ClientId = 3, Client = clients.FirstOrDefault(c => c.ClientId == 3)},
                new Job { JobId = 4, JobType = "After-Builder Cleaning", Price = 150.00m, Date = DateTime.Now.AddDays(-6), Status = "Cancelled", Address = "3 Elm Drive, Limerick", JobDescription = "Post renovation deep clean", TimeTaken = "3 hours", ClientId = 4, Client = clients.FirstOrDefault(c => c.ClientId == 4)},
                new Job { JobId = 5, JobType = "Upholstery Cleaning", Price = 95.00m, Date = DateTime.Now.AddDays(1), Status = "Scheduled", Address = "22 Birch Lane, Waterford", JobDescription = "Sofa and armchairs", TimeTaken = "1.5 hours", ClientId = 5, Client = clients.FirstOrDefault(c => c.ClientId == 5)}
            };

            dgJobs.ItemsSource = jobs;
            dgClients.ItemsSource = clients;
        }

        
    }
}
