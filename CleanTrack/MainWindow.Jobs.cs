using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CleanTrack
{
    public partial class MainWindow
    {
       //----- JOBS TAB -----

        private void FilterJobResults()
        {
            if (tbxSearchJobs == null) return; // guard against being called before InitializeComponent finishes

            try
            {
                var filtered = db.Jobs.AsQueryable();

                // filter by search
                string search = tbxSearchJobs.Text.ToLower();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    filtered = filtered.Where(j =>
                        j.Address.ToLower().Contains(search) ||
                        j.Client.FirstName.ToLower().Contains(search) ||
                        j.Client.LastName.ToLower().Contains(search) ||
                        (j.Client.FirstName + " " + j.Client.LastName).ToLower().Contains(search) ||
                        j.JobDescription.ToLower().Contains(search));
                }

                // filter by status
                if (cbxStatusFilter.SelectedItem is ComboBoxItem statusItem && statusItem.Content.ToString() != "All")
                    filtered = filtered.Where(j => j.Status == statusItem.Content.ToString());

                // filter by job type
                if (cbxJobTypeFilter.SelectedItem is ComboBoxItem typeItem && typeItem.Content.ToString() != "All")
                    filtered = filtered.Where(j => j.JobType == typeItem.Content.ToString());

                // filter by employee — FullName is a computed prop EF can't translate, so split and compare FirstName/LastName
                if (cbxEmployeeFilter.SelectedItem is ComboBoxItem employeeItem && employeeItem.Content.ToString() != "All")
                {
                    string[] nameParts = employeeItem.Content.ToString().Split(' ');
                    string firstName = nameParts[0];
                    string lastName = nameParts.Length > 1 ? nameParts[1] : "";
                    filtered = filtered.Where(j => j.Employees.Any(e => e.FirstName == firstName && e.LastName == lastName));
                }

                // filter by date range
                if (dpFromDateFilter.SelectedDate.HasValue)
                    filtered = filtered.Where(j => j.Date >= dpFromDateFilter.SelectedDate.Value);
                if (dpToDateFilter.SelectedDate.HasValue)
                    filtered = filtered.Where(j =>
                    j.Date <= dpToDateFilter.SelectedDate.Value);

                // inlude - makes sure EF loads the Client and Employees data so we can display it in the DataGrid without lazy loading issues
                var results = filtered.Include("Client").Include("Employees").ToList();

                results.Sort(); // uses Job.CompareTo — sorts by date ascending (IComparable<Job>)

                dgJobs.ItemsSource = null;
                dgJobs.ItemsSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to filter jobs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //--- filter event listeners ---
        private void tbxSearchJobs_TextChanged(object sender, TextChangedEventArgs e)       => FilterJobResults();
        private void cbxStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)  => FilterJobResults();
        private void cbxJobTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => FilterJobResults();
        private void cbxEmployeeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => FilterJobResults();
        private void dpFromDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => FilterJobResults();
        private void dpToDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)   => FilterJobResults();


        //--- action buttons ---
        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add job: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteJob_Click(object sender, RoutedEventArgs e)
        {
            if (dgJobs.SelectedItem == null)
            {
                MessageBox.Show("Please select a job to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Job selectedJob = dgJobs.SelectedItem as Job;

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete this job?\n\n{selectedJob.JobType} at {selectedJob.Address}",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.Jobs.Remove(selectedJob);
                    db.SaveChanges();
                    FilterJobResults();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete job: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnEditJob_Click(object sender, RoutedEventArgs e)
        {
            if (dgJobs.SelectedItem == null)
            {
                MessageBox.Show("Please select a job to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Job selectedJob = dgJobs.SelectedItem as Job;
                EditJobWindow editJobWindow = new EditJobWindow(selectedJob, db.Clients.ToList(), db.Employees.ToList());
                editJobWindow.Owner = this;

                if (editJobWindow.ShowDialog() == true)
                {
                    db.SaveChanges();
                    FilterJobResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save job changes: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

    }
}
