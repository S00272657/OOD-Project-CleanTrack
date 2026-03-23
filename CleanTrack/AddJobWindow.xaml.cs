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
    /// Interaction logic for AddJobWindow.xaml
    /// </summary>
    public partial class AddJobWindow : Window
    {
        public Job NewJob { get; set; }
        private List<Employee> _employees;
        private List<Client> _clients;

        public AddJobWindow(List<Client> clients, List<Employee> employees)
        {
            InitializeComponent();
            _clients = clients;
            _employees = employees;

            // populate dropdowns
            cbxClient.ItemsSource = _clients;
            lbxEmployees.ItemsSource = _employees;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // validation
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
            if (cbxStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // get selected employees from checklist
            var selectedEmployees = lbxEmployees.SelectedItems.Cast<Employee>().ToList();

            NewJob = new Job
            {
                JobType = (cbxJobType.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Client = cbxClient.SelectedItem as Client,
                ClientId = (cbxClient.SelectedItem as Client).ClientId,
                Address = tbxAddress.Text,
                Date = dpJobDate.SelectedDate.Value,
                Price = price,
                TimeTaken = tbxTimeTaken.Text,
                Status = (cbxStatus.SelectedItem as ComboBoxItem)?.Content.ToString(),
                JobDescription = tbxDescription.Text,
                Employees = selectedEmployees
            };
            
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
