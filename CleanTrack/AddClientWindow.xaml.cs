using System.Windows;

namespace CleanTrack
{
    public partial class AddClientWindow : Window
    {
        public Client NewClient { get; set; }

        public AddClientWindow()
        {
            InitializeComponent();
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
            if (string.IsNullOrWhiteSpace(tbxAddress.Text))
            {
                MessageBox.Show("Please enter an address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            NewClient = new Client
            {
                FirstName   = tbxFirstName.Text,
                LastName    = tbxLastName.Text,
                PhoneNumber = tbxPhone.Text,
                Email       = tbxEmail.Text,
                Address     = tbxAddress.Text,
                City        = tbxCity.Text,
                Eircode     = tbxEircode.Text,
                IsActive    = chkIsActive.IsChecked == true
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
