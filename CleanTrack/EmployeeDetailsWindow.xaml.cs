using System.Windows;

namespace CleanTrack
{
    public partial class EmployeeDetailsWindow : Window
    {
        public EmployeeDetailsWindow(Employee employee)
        {
            InitializeComponent();
            LoadEmployeeDetails(employee);
        }

        private void LoadEmployeeDetails(Employee employee)
        {
            txtName.Text      = employee.FullName;
            txtPhone.Text     = employee.PhoneNumber;
            txtEmail.Text     = employee.Email;
            txtRole.Text      = employee.Role;
            txtStartDate.Text = employee.StartDate.ToString("dd/MM/yyyy");
            txtStatus.Text    = employee.ActiveStatus;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
