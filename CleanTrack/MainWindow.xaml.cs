using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace CleanTrack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext db = new AppDbContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            dgJobs.ItemsSource = db.Jobs.Include("Client").Include("Employees").ToList();
            dgClients.ItemsSource = db.Clients.ToList();
            dgEmployees.ItemsSource = db.Employees.ToList();
        }


        #region JobsTab
        //JOB OBJECT: JobId, JobType, Price, Date, Status, Address, JobDescription, TimeTaken, ClientId, Client, Employees
        //I want to display: Date, JobType, Price, Client.Name, Status, Address, JobDescription, timetaken, employee

        private void FilterJobResults()
        {
            var filtered = db.Jobs.AsQueryable();

            //filter by search 
            string search = tbxSearchJobs.Text.ToLower();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(j =>
                    j.Address.ToLower().Contains(search) ||
                    j.Client.FirstName.ToLower().Contains(search) ||
                    j.Client.LastName.ToLower().Contains(search) ||
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
            AddJobWindow addJobWindow = new AddJobWindow(db.Clients.ToList(), db.Employees.ToList());
            addJobWindow.Owner = this;

            if (addJobWindow.ShowDialog() == true)
            {
                db.Jobs.Add(addJobWindow.NewJob);
                db.SaveChanges();
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
                db.Jobs.Remove(selectedJob);
                db.SaveChanges();
                FilterJobResults();
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

            EditJobWindow editJobWindow = new EditJobWindow(selectedJob, db.Clients.ToList(), db.Employees.ToList());
            editJobWindow.Owner = this;

            if (editJobWindow.ShowDialog() == true)
            {
                db.SaveChanges();
                FilterJobResults();
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
            var filtered = db.Employees.AsQueryable();

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
                db.Employees.Remove(selectedEmployee);
                db.SaveChanges();
                FilterEmployeeResults();
            }
        }

        private void btnViewEmployeeDetails_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #endregion



        
    }
}
