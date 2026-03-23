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
    /// Interaction logic for JobDetailsWindow.xaml
    /// </summary>
    public partial class JobDetailsWindow : Window
    {
        public JobDetailsWindow(Job job)
        {
            InitializeComponent();
            LoadJobDetails(job);
        }

        private void LoadJobDetails(Job job)
        {
            // job details
            txtJobType.Text = job.JobType;
            txtStatus.Text = job.Status;
            txtDate.Text = job.Date.ToString("dd/MM/yyyy");
            txtAddress.Text = job.Address;
            txtPrice.Text = $"€{job.Price:F2}";
            txtTimeTaken.Text = job.TimeTaken;
            txtDescription.Text = job.JobDescription;

            // client details
            if (job.Client != null)
            {
                txtClientName.Text = job.Client.FullName;
                txtClientPhone.Text = job.Client.PhoneNumber;
                txtClientEmail.Text = job.Client.Email;
                txtClientAddress.Text = job.Client.Address;
                txtClientCity.Text = job.Client.City;
            }
            else
            {
                txtClientName.Text = "No client assigned";
            }

            // employees
            if (job.Employees != null && job.Employees.Any())
            {
                lbxAssignedEmployees.ItemsSource = job.Employees;
            }
            else
            {
                lbxAssignedEmployees.Items.Add(new TextBlock { Text = "No employees assigned" });
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
