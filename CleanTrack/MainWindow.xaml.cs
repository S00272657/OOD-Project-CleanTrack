using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace CleanTrack
{
    public partial class MainWindow : Window
    {
        private AppDbContext db = new AppDbContext();
        private DateTime _weekStart;

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
            try
            {
                dgJobs.ItemsSource      = db.Jobs.Include("Client").Include("Employees").ToList();
                dgClients.ItemsSource   = db.Clients.ToList();
                dgEmployees.ItemsSource = db.Employees.ToList();


                cbxEmployeeFilter.Items.Clear();
                //add All as first item to cbx
                cbxEmployeeFilter.Items.Add(new System.Windows.Controls.ComboBoxItem { Content = "All" });
                //add each employee as an item
                foreach (var emp in db.Employees.ToList())
                    cbxEmployeeFilter.Items.Add(new System.Windows.Controls.ComboBoxItem { Content = emp.FullName });
                //set All as default
                cbxEmployeeFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
