using System.Windows;
using System.Windows.Controls;

namespace CleanTrack
{
    public partial class EditEmployeeWindow : Window
    {
        private Employee _existingEmployee;

        public EditEmployeeWindow(Employee employee)
        {
            InitializeComponent();
            _existingEmployee = employee;
            PreFillFields();
        }

        private void PreFillFields()
        {
            tbxFirstName.Text        = _existingEmployee.FirstName;
            tbxLastName.Text         = _existingEmployee.LastName;
            tbxPhone.Text            = _existingEmployee.PhoneNumber;
            tbxEmail.Text            = _existingEmployee.Email;
            dpStartDate.SelectedDate = _existingEmployee.StartDate;
            chkIsActive.IsChecked    = _existingEmployee.IsActive;

            foreach (ComboBoxItem item in cbxRole.Items)
            {
                if (item.Content.ToString() == _existingEmployee.Role)
                {
                    cbxRole.SelectedItem = item;
                    break;
                }
            }
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

            _existingEmployee.FirstName   = tbxFirstName.Text;
            _existingEmployee.LastName    = tbxLastName.Text;
            _existingEmployee.PhoneNumber = tbxPhone.Text;
            _existingEmployee.Email       = tbxEmail.Text;
            _existingEmployee.Role        = (cbxRole.SelectedItem as ComboBoxItem)?.Content.ToString();
            _existingEmployee.StartDate   = dpStartDate.SelectedDate.Value;
            _existingEmployee.IsActive    = chkIsActive.IsChecked == true;

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
