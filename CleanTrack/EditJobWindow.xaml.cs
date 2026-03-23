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
using System.Windows.Shapes;

namespace CleanTrack
{
    /// <summary>
    /// Interaction logic for EditJobWindow.xaml
    /// </summary>
    public partial class EditJobWindow : Window
    {
        public Job UpdatedJob { get; set; }
        private List<Employee> _employees;
        private List<Client> _clients;
        private Job _existingJob;

        public EditJobWindow(Job job, List<Client> clients, List<Employee> employees)
        {
            InitializeComponent();
            _existingJob = job;
            _clients = clients;
            _employees = employees;

            cbxClient.ItemsSource = _clients;
            lbxEmployees.ItemsSource = _employees;

            // pre-fill all fields with existing job data
            PreFillFields();
        }

        private void PreFillFields()
        {
            // set job type
            foreach (ComboBoxItem item in cbxJobType.Items)
            {
                if (item.Content.ToString() == _existingJob.JobType)
                {
                    cbxJobType.SelectedItem = item;
                    break;
                }
            }

            // set client
            cbxClient.SelectedItem = _clients.FirstOrDefault(c => c.ClientId == _existingJob.ClientId);

            // set text fields
            tbxAddress.Text = _existingJob.Address;
            dpJobDate.SelectedDate = _existingJob.Date;
            tbxPrice.Text = _existingJob.Price.ToString();
            tbxTimeTaken.Text = _existingJob.TimeTaken;
            tbxDescription.Text = _existingJob.JobDescription;

            // set status
            foreach (ComboBoxItem item in cbxStatus.Items)
            {
                if (item.Content.ToString() == _existingJob.Status)
                {
                    cbxStatus.SelectedItem = item;
                    break;
                }
            }

            // pre-select assigned employees
            for (int i = 0; i < lbxEmployees.Items.Count; i++)
            {
                Employee emp = lbxEmployees.Items[i] as Employee;
                if (_existingJob.Employees.Any(e => e.EmployeeId == emp.EmployeeId))
                {
                    lbxEmployees.SelectedItems.Add(lbxEmployees.Items[i]);
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbxJobType.SelectedItem == null)
            {
                MessageBox.Show("Please select a job type.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cbxClient.SelectedItem == null)
            {
                MessageBox.Show("Please select a client.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxAddress.Text))
            {
                MessageBox.Show("Please enter an address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (dpJobDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!decimal.TryParse(tbxPrice.Text, out decimal price))
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // update the existing job object directly
            _existingJob.JobType = (cbxJobType.SelectedItem as ComboBoxItem)?.Content.ToString();
            _existingJob.Client = cbxClient.SelectedItem as Client;
            _existingJob.ClientId = (_existingJob.Client).ClientId;
            _existingJob.Address = tbxAddress.Text;
            _existingJob.Date = dpJobDate.SelectedDate.Value;
            _existingJob.Price = price;
            _existingJob.TimeTaken = tbxTimeTaken.Text;
            _existingJob.Status = (cbxStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            _existingJob.JobDescription = tbxDescription.Text;
            _existingJob.Employees = lbxEmployees.SelectedItems.Cast<Employee>().ToList();

            UpdatedJob = _existingJob;
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
