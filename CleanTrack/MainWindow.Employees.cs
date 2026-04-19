using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CleanTrack
{
    public partial class MainWindow
    {
        #region EmployeesTab

        private void FilterEmployeeResults()
        {
            if (tbxSearchEmployees == null) return; // guard against firing before InitializeComponent finishes

            try
            {
                var filtered = db.Employees.AsQueryable();

                // filter by search
                string search = tbxSearchEmployees.Text.ToLower();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    filtered = filtered.Where(e =>
                        e.FirstName.ToLower().Contains(search) ||
                        e.LastName.ToLower().Contains(search) ||
                        e.Email.ToLower().Contains(search) ||
                        e.PhoneNumber.Contains(search));
                }

                // filter by role
                if (cbxRoleFilter.SelectedItem is ComboBoxItem roleItem && roleItem.Content.ToString() != "All")
                    filtered = filtered.Where(e => e.Role == roleItem.Content.ToString());

                // filter by active status
                if (cbxActiveFilter.SelectedItem is ComboBoxItem activeItem && activeItem.Content.ToString() != "All")
                {
                    bool isActive = activeItem.Content.ToString() == "Active";
                    filtered = filtered.Where(e => e.IsActive == isActive);
                }

                dgEmployees.ItemsSource = null;
                dgEmployees.ItemsSource = filtered.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to filter employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region FilterEventListeners
        private void tbxSearchEmployees_SelectionChanged(object sender, RoutedEventArgs e) => FilterEmployeeResults();
        private void cbxRoleFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)   => FilterEmployeeResults();
        private void cbxActiveFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) => FilterEmployeeResults();
        #endregion

        #region ActionButtons
        private void btnResetEmployeeFilters_Click(object sender, RoutedEventArgs e)
        {
            tbxSearchEmployees.Text = string.Empty;
            cbxRoleFilter.SelectedIndex = 0;
            cbxActiveFilter.SelectedIndex = 0;
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
                addEmployeeWindow.Owner = this;

                if (addEmployeeWindow.ShowDialog() == true)
                {
                    db.Employees.Add(addEmployeeWindow.NewEmployee);
                    db.SaveChanges();
                    FilterEmployeeResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add employee: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Employee selectedEmployee = dgEmployees.SelectedItem as Employee;
                EditEmployeeWindow editEmployeeWindow = new EditEmployeeWindow(selectedEmployee);
                editEmployeeWindow.Owner = this;

                if (editEmployeeWindow.ShowDialog() == true)
                {
                    db.SaveChanges();
                    FilterEmployeeResults();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save employee changes: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    db.Employees.Remove(selectedEmployee);
                    db.SaveChanges();
                    FilterEmployeeResults();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete employee: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnViewEmployeeDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgEmployees.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee to view.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Employee selectedEmployee = dgEmployees.SelectedItem as Employee;
            EmployeeDetailsWindow detailsWindow = new EmployeeDetailsWindow(selectedEmployee);
            detailsWindow.Owner = this;
            detailsWindow.ShowDialog();
        }
        #endregion

        #endregion
    }
}
