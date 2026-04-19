using System;
using System.Windows;
using System.Windows.Controls;

namespace CleanTrack
{
    public partial class AddEmployeeWindow : Window
    {
        public Employee NewEmployee { get; set; }

        public AddEmployeeWindow()
        {
            InitializeComponent();
            dpStartDate.SelectedDate = DateTime.Today;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxFirstName.Text))
            {
                MessageBox.Show("Please enter a first name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxLastName.Text))
            {
                MessageBox.Show("Please enter a last name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxPhone.Text))
            {
                MessageBox.Show("Please enter a phone number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbxEmail.Text))
            {
                MessageBox.Show("Please enter an email.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cbxRole.SelectedItem == null)
            {
                MessageBox.Show("Please select a role.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (dpStartDate.SelectedDate == null)
            {
                MessageBox.Show("Please select a start date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewEmployee = new Employee
            {
                FirstName = tbxFirstName.Text,
                LastName  = tbxLastName.Text,
                PhoneNumber = tbxPhone.Text,
                Email     = tbxEmail.Text,
                Role      = (cbxRole.SelectedItem as ComboBoxItem)?.Content.ToString(),
                StartDate = dpStartDate.SelectedDate.Value,
                IsActive  = chkIsActive.IsChecked == true
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
